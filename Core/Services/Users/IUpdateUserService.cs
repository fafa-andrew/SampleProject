using BusinessEntities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Services.Users
{
    public interface IUpdateUserService
    {
        Task UpdateAsync(
                    User user,
                    string name,
                    string email,
                    int age,
                    UserTypes type,
                    decimal? annualSalary,
                    IEnumerable<string> tags,
                    CancellationToken ct
            );
    }
}