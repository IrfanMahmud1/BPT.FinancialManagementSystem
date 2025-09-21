using BPT.FMS.Domain.Entities;
using BPT.FMS.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.FMS.Infrastructure.Repositories
{
    public abstract class Repository<TEntity,TKey> : IRepository<TEntity,TKey> 
        where TEntity : class, IEntity<TKey> 
        where TKey : IEquatable<TKey>
    {
        private DbContext _dbContext;
        private DbSet<TEntity> _dbSet;

        public Repository(DbContext context)
        {
            _dbContext = context;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual async Task RemoveAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                throw new ArgumentException($"Entity with id not found");
            }
            await RemoveAsync(entity);
        }

        private async Task RemoveAsync(TEntity entityToDelete)
        {
            await Task.Run(() =>
            {
                if (_dbContext.Entry(entityToDelete).State == EntityState.Detached)
                {
                    _dbSet.Attach(entityToDelete);
                }
                _dbSet.Remove(entityToDelete);
            });
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            IQueryable<TEntity> query = _dbSet;
            return await query.ToListAsync();
        }

        public virtual async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task EditAsync(TEntity entity)
        {
            await Task.Run(() =>
            {
                _dbSet.Attach(entity);
                _dbContext.Entry(entity).State = EntityState.Modified;
            });
        }
    }
}
