using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartHome.Model.Entities;

namespace SmartHome.Model
{
    public interface IRepository<TEntity> where TEntity: BaseEntity
    {
        Task<TEntity> GetAsync(long id);
        Task<IEnumerable<TEntity>> GetAsync(Func<TEntity, bool> filter);
        Task CreateAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(long id);
    }
}
