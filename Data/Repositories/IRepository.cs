using System;
using System.Threading;
using System.Threading.Tasks;
using BusinessEntities;

namespace Data.Repositories
{
    public interface IRepository<T> where T : IdObject
    {
        Task StoreAsync(T entity, CancellationToken ct);
        Task SaveAsync(CancellationToken ct);
        Task DeleteAsync(T entity);
        Task<T> GetAsync(Guid id, CancellationToken ct);
    }
}