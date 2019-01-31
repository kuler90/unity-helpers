using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelpers.ServiceLocator
{
    public static class ServiceLocator
    {
        private readonly static Dictionary<string, Func<object>> resolvers = new Dictionary<string, Func<object>>();
        private readonly static Dictionary<string, object> singletons = new Dictionary<string, object>();

        public static void RegisterType<T, R>(bool singleton = false, string domain = null)
            where T : class
            where R : class, T, new()
        {
            RegisterFactory<T, R>(() => new R(), singleton, domain);
        }

        public static void RegisterType<T>(bool singleton = false, string domain = null)
            where T : class, new()
        {
            RegisterType<T, T>(singleton, domain);
        }

        public static void RegisterMonoBehavior<T, R>(Action<R> initializer = null, bool singleton = false, string resourcePath = null, string domain = null)
            where T : class
            where R : MonoBehaviour, T
        {
            RegisterFactory<T, R>(() =>
            {
                GameObject gameObject;
                // Try create from prefab
                GameObject prefab = Resources.Load<GameObject>(resourcePath ?? typeof(R).Name);
                if (prefab != null)
                {
                    gameObject = UnityEngine.Object.Instantiate(prefab);
                }
                else
                {
                    // Create new game object
                    gameObject = new GameObject();
                    gameObject.AddComponent<R>();
                }
                gameObject.name = KeyFor(typeof(T), domain);
                return gameObject.GetComponent<R>();
            }, singleton, domain);
        }

        public static void RegisterMonoBehavior<T>(Action<T> initializer = null, bool singleton = false, string resourcePath = null, string domain = null)
            where T : MonoBehaviour
        {
            RegisterMonoBehavior<T, T>(initializer, singleton, resourcePath, domain);
        }

        public static void RegisterInstance<T>(T instance, bool weak = false, string domain = null)
            where T : class
        {
            if (weak)
            {
                WeakReference weakRef = new WeakReference(instance);
                RegisterFactory<T>(() => weakRef.Target as T, false, domain);
            }
            else
            {
                string key = KeyFor(typeof(T), domain);
                RegisterFactory<T>(() => singletons[key] as T, false, domain);
                singletons[key] = instance;
                if (instance is MonoBehaviour)
                {
                    UnityEngine.Object.DontDestroyOnLoad((instance as MonoBehaviour).gameObject);
                }
            }
        }

        public static void RegisterFactory<T, R>(Func<R> constructor, bool singleton = false, string domain = null)
            where T : class
            where R : class, T
        {
            // Check if registration already exists
            if (Contains<T>(domain))
            {
                Remove<T>(domain);
                Debug.LogWarning("[ServiceLocator] Registration for type " + typeof(T) + " is overwritten");
            }

            // Create resolver for type
            string key = KeyFor(typeof(T), domain);
            Func<R> resolver;
            if (singleton)
            {
                resolver = () =>
                {
                    if (singletons[key] == null)
                    {
                        // Create and save singleton instance
                        var instance = constructor();
                        if (instance is MonoBehaviour)
                        {
                            UnityEngine.Object.DontDestroyOnLoad((instance as MonoBehaviour).gameObject);
                        }
                        Debug.Log("[ServiceLocator] Singleton instance of " + typeof(R) + " was created for type " + typeof(T));
                        singletons[key] = instance;
                    }
                    return singletons[key] as R;
                };
            }
            else
            {
                resolver = () =>
                {
                    var instance = constructor();
                    Debug.Log("[ServiceLocator] New instance of " + typeof(R) + " was created for type " + typeof(T));
                    return instance;
                };
            }

            // Save resolver with thread-safety handling
            Monitor.Enter(resolvers);
            resolvers[key] = resolver;
            Monitor.Exit(resolvers);
            Debug.Log("[ServiceLocator] Registration for type " + typeof(T) + " was created");
        }

        public static void RegisterFactory<T>(Func<T> constructor, bool singleton = false, string domain = null)
            where T : class
        {
            RegisterFactory<T, T>(constructor, singleton, domain);
        }

        public static bool Contains<T>(string domain = null)
        {
            string key = KeyFor(typeof(T), domain);
            return resolvers.ContainsKey(key);
        }

        public static IEnumerator WaitForRegister<T>(string domain = null)
        {
            while (!Contains<T>(domain))
            {
                yield return null;
            }
        }

        public static T Resolve<T>(string domain = null)
            where T : class
        {
            string key = KeyFor(typeof(T), domain);
            T instance = null;
            // Get instance with thread-safety handling
            Monitor.Enter(resolvers);
            var resolver = resolvers[key];
            if (resolver != null)
            {
                instance = resolver() as T;
            }
            else
            {
                Debug.LogError("[ServiceLocator] Registration for type " + typeof(T) + " not found");
            }
            Monitor.Exit(resolvers);
            return instance;
        }

        public static void Remove<T>(string domain = null)
        {
            string key = KeyFor(typeof(T), domain);
            // Remove registration with thread-safety handling
            Monitor.Enter(resolvers);
            if (singletons[key] != null && singletons[key] is MonoBehaviour)
            {
                // Destroy MonoBehaviour singleton
                UnityEngine.Object.Destroy((singletons[key] as MonoBehaviour).gameObject);
                Debug.Log("[ServiceLocator] Singleton MonoBehaviour instance of " + singletons[key].GetType() + " for type " + typeof(T) + " was destroyed");
            }
            singletons[key] = null;
            resolvers[key] = null;
            Monitor.Exit(resolvers);
            Debug.Log("[ServiceLocator] Registration for type " + typeof(T) + " was removed");
        }

        private static string KeyFor(Type type, string domain)
        {
            return type.Name + (domain != null ? "@" + domain : "");
        }
    }
}
