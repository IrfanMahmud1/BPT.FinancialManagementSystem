using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.FMS.Domain.Repositories
{
    public interface IRepository<TEntity, TKey>
        where TEntity : class
        where TKey : IEquatable<TKey>
    {
        Task<TEntity> GetByIdAsync(Guid id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task AddAsync(TEntity entity);
        Task EditAsync(TEntity entity);
        Task RemoveAsync(Guid id);
    }
}
