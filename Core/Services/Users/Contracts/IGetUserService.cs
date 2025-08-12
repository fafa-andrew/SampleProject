using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BusinessEntities;

namespace Core.Services.Users.Contracts
{
    public interface IGetUserService
    {
        Task<User> GetUserAync(Guid id, CancellationToken ct);

        Task<IEnumerable<User>> GetUsersAsync(
            CancellationToken ct,
            UserTypes? userType = null, 
            string name = null,
            string email = null
            );
    }
}