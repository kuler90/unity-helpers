
namespace UnityHelpers.Storage
{
    public class SingleRepository<T>
    {
        private ICache cache;
        private string key = typeof(T).Name;

        public SingleRepository(ICache cache)
        {
            this.cache = cache;
        }

        public T Get()
        {
            return (T)cache.GetObject(key);
        }

        public void Set(T data)
        {
            cache.SetObject(key, data);
        }
    }
}