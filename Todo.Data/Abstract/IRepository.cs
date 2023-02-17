using System.Collections.Generic;
using System.Threading.Tasks;

namespace Todo.Data.Abstract
{
    public interface IRepository<TEntity>
         where TEntity : class
    {
        Task CreateAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task<List<TEntity>> GetAllAsync(int limit, int offset);
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(int id);
    }

}