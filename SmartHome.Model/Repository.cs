using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using SmartHome.Model.Entities;

namespace SmartHome.Model
{
    public class Repository<TEntity>: IRepository<TEntity>
    where TEntity: BaseEntity
    {
        private readonly ConcurrentDictionary<long, TEntity> _entitiesMap;

        public Repository()
        {
            _entitiesMap = new ConcurrentDictionary<long, TEntity>();
        }

        public Repository(IReadOnlyCollection<TEntity> entities)
        {
            _entitiesMap = new ConcurrentDictionary<long, TEntity>();

            foreach (TEntity entity in entities)
            {
                _entitiesMap[entity.Id] = entity;
            }
        }

        public Task<TEntity> GetAsync(long id)
        {
            _entitiesMap.TryGetValue(id, out var entity);
            return Task.FromResult(entity);
        }

        public Task<IEnumerable<TEntity>> GetAsync(Func<TEntity, bool> filter)
        {
            var entities = _entitiesMap.Values.Where(filter);
            return Task.FromResult(entities);
        }

        public Task CreateAsync(TEntity entity)
        {
            var id = GetNextId();

           if(!_entitiesMap.TryAdd(id, entity))
               throw new DBConcurrencyException("Не удалось создать сущность");

            entity.Id = id;
            return Task.CompletedTask;
        }

        public Task UpdateAsync(TEntity entity)
        {
            if(!_entitiesMap.TryGetValue(entity.Id, out _))
                throw new Exception("Сущность не найдена");

            _entitiesMap[entity.Id] = entity;

            return Task.CompletedTask;
        }

        public Task DeleteAsync(long id)
        {
           if(!_entitiesMap.TryRemove(id, out _)) 
               throw new Exception("Ошибка удаления");

           return Task.CompletedTask;
        }

        private long GetNextId()
        {
            lock (_entitiesMap)
            {
                return _entitiesMap.Keys.Max() + 1;
            }
        }
    }
}
