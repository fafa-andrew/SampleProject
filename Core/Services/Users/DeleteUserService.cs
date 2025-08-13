using BusinessEntities;
using Common;
using Core.Services.Users.Contracts;
using Data.Repositories.Contracts;
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

        public async Task DeleteAsync(User user)
        {
            await _userRepository.DeleteAsync(user);
        }

        public async Task DeleteAllAsync()
        {
            await _userRepository.DeleteAllAsync();
        }
    }
}