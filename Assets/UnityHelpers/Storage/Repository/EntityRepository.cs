using System.Linq;
using System.Collections.Generic;

namespace UnityHelpers.Storage
{
    public class EntityRepository<T> where T : IEntity
    {
        private ICache cache;

        public EntityRepository(ICache cache)
        {
            this.cache = cache;
        }

        public IEnumerable<T> EnumerateAll()
        {
            return cache.EnumerateKeys().Select(GetById);
        }

        public List<T> GetAll()
        {
            return EnumerateAll().ToList();
        }

        public T GetById(string id)
        {
            return (T)cache.GetObject(id);
        }

        public void Save(T entity)
        {
            cache.SetObject(entity.Id, entity);
        }

        public void Remove(T entity)
        {
            cache.Remove(entity.Id);
        }

        public void Clear()
        {
            cache.Clear();
        }

        public int Count()
        {
            return cache.EnumerateKeys().Count();
        }
    }
}