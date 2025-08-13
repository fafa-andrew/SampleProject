using Common;
using Data.Repositories.Contracts;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Data.Repositories
{
    [AutoRegister]
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;

        public GenericRepository(AppDbContext context) 
            => _context = context ?? throw new ArgumentNullException(nameof(context));

        public IQueryable<T> GetAll => _context.Set<T>();

        public async Task<T> FindAsync(Guid id) => await _context.Set<T>().FindAsync(id);

        public async Task AddAsync(T entity) => await _context.Set<T>().AddAsync(entity);

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity != null) _context.Set<T>().Remove(entity);
        }

        public async Task SaveAsync(CancellationToken ct) => await _context.SaveChangesAsync(ct);

    }
}
