using System;
using System.Linq;
using System.Collections.Generic;

namespace UnityHelpers.Storage
{
    public class ListRepository<T>
    {
        private ICache cache;
        private SingleRepository<List<string>> keysRepository;

        public ListRepository(ICache cache)
        {
            this.cache = cache;
            keysRepository = new SingleRepository<List<string>>(cache);
            keysRepository.Set(new List<string>());
        }

        public IEnumerable<T> EnumerateAll()
        {
            return keysRepository.Get()
                .Where(cache.ContainsKey).Select(cache.GetObject).Cast<T>();
        }

        public List<T> GetAll()
        {
            lock (keysRepository)
            {
                return EnumerateAll().ToList();
            }
        }

        public T Get(int index)
        {
            lock (keysRepository)
            {
                var keys = keysRepository.Get();
                var key = keys[index];
                return (T)cache.GetObject(key);
            }
        }

        public void Insert(int index, T entity)
        {
            lock (keysRepository)
            {
                var key = Guid.NewGuid().ToString();
                var keys = keysRepository.Get();
                try
                {
                    keys.Insert(index, key);
                    keysRepository.Set(keys);
                    cache.SetObject(key, entity);
                }
                catch (Exception e)
                {
                    keys.RemoveAt(index);
                    keysRepository.Set(keys);
                    cache.Remove(key);
                    throw e;
                }
            }

        }

        public void Add(T entity)
        {
            lock (keysRepository)
            {
                var key = Guid.NewGuid().ToString();
                var keys = keysRepository.Get();
                var insertIndex = keys.Count;
                try
                {
                    keys.Insert(insertIndex, key);
                    keysRepository.Set(keys);
                    cache.SetObject(key, entity);
                }
                catch (Exception e)
                {
                    keys.RemoveAt(insertIndex);
                    keysRepository.Set(keys);
                    cache.Remove(key);
                    throw e;
                }
            }
        }

        public void Remove(int index)
        {
            lock (keysRepository)
            {
                var keys = keysRepository.Get();
                var key = keys[index];
                keys.RemoveAt(index);
                keysRepository.Set(keys);
                cache.Remove(key);
            }
        }

        public void Clear()
        {
            lock (keysRepository)
            {
                keysRepository.Set(new List<string>());
                cache.Clear();
            }
        }

        public int Count()
        {
            return keysRepository.Get().Count();
        }
    }
}