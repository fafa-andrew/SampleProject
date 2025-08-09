using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BusinessEntities;
using Common;
using Data.Repositories;

namespace Core.Services.Users
{
    [AutoRegister]
    public class GetUserService : IGetUserService
    {
        private readonly IUserRepository _userRepository;

        public GetUserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetUserAync(Guid id, CancellationToken ct)
        {
            return await _userRepository.GetAsync(id, ct);
        }

        public async Task<IEnumerable<User>> GetUsersAsync(
            CancellationToken ct,
            UserTypes? userType = null, 
            string name = null, 
            string email = null
            )
        {
            return await _userRepository.GetAsync(ct, userType, name, email);
        }
    }
}