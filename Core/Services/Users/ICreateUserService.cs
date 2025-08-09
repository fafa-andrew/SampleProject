using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BusinessEntities;

namespace Core.Services.Users
{
    public interface ICreateUserService
    {
       Task<User> CreateAsync(
            Guid id, 
            string name,
            string email,
            UserTypes type, 
            decimal? annualSalary, 
            IEnumerable<string> tags,
            CancellationToken ct
            );
    }
}