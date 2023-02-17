using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Data.Abstract;
using Todo.Entity.Abstract;

namespace Todo.Data.Concrete
{
    public abstract class PgGenericRepository<TEntity> : IRepository<TEntity>
          where TEntity : BaseEntity
    {
        public readonly DatabaseContexts _context;

        public PgGenericRepository(DatabaseContexts context)
        {
            _context = context;
        }
        public async Task CreateAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<TEntity>> GetAllAsync(int limit, int offset)
        {
            return await _context.Set<TEntity>()
                .OrderBy(e => e.Created_at)
                .Skip(offset)
                .Take(limit + offset)
                .ToListAsync();
        }
        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>()
                .OrderBy(e => e.Created_at)
                .ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _context.Set<TEntity>().Where(e => e.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync();
        }


    }
}
