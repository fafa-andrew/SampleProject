using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BusinessEntities;

namespace Data.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<IEnumerable<User>> GetAsync(
            CancellationToken ct,
            UserTypes? userType = null,
            string name = null, 
            string email = null);

        Task DeleteAllAsync();
    }
}