using BusinessEntities;
using Common;
using Data.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Services.Users
{
    [AutoRegister]
    public class DeleteUserService : IDeleteUserService
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task DeleteAsync(User user, CancellationToken ct)
        {
            await _userRepository.DeleteAsync(user);
            await _userRepository.SaveAsync(ct);
        }

        public async Task DeleteAllAsync(CancellationToken ct)
        {
            await _userRepository.DeleteAllAsync(ct);
        }
    }
}