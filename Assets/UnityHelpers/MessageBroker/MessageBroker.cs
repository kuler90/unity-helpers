using System;
using System.Collections.Generic;

namespace UnityHelpers.MessageBroker
{
    public class MessageBroker
    {
        public readonly static MessageBroker Default = new MessageBroker();

        private Dictionary<Type, List<Action<object>>> observations = new Dictionary<Type, List<Action<object>>>();

        public void Publish<T>(T message)
        {
            Type key = typeof(T);
            var observers = observations[key] ?? new List<Action<object>>();
            foreach (var observer in new List<Action<object>>(observers))
            {
                observer(message);
            }
        }

        public void AddObserver<T>(Action<T> observer)
        {
            lock (observations)
            {
                Type key = typeof(T);
                var observers = observations[key] ?? new List<Action<object>>();
                observers.Add(observer as Action<object>);
                observations[key] = observers;
            }
        }

        public void RemoveObserver<T>(Action<T> observer)
        {
            lock (observations)
            {
                Type key = typeof(T);
                var observers = observations[key] ?? new List<Action<object>>();
                observers.Remove(observer as Action<object>);
                observations[key] = observers;
            }
        }
    }
}