using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Data.Repositories.Contracts
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll { get; }

        Task<T> FindAsync(Guid id);

        Task AddAsync(T entity);

        Task DeleteAsync(Guid id);

        Task SaveAsync(CancellationToken ct);
    }
}