using BusinessEntities;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Services.Users.Contracts
{
    public interface IDeleteUserService
    {
        Task DeleteAsync(User user);
        Task DeleteAllAsync();
    }
}