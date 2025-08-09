using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BusinessEntities;
using Common;

namespace Core.Services.Users
{
    [AutoRegister(AutoRegisterTypes.Singleton)]
    public class UpdateUserService : IUpdateUserService
    {
        public async Task UpdateAsync(
            User user, 
            string name, 
            string email,
            UserTypes type,
            decimal? annualSalary,
            IEnumerable<string> tags,
            CancellationToken ct)
        {
            user.SetEmail(email);
            user.SetName(name);
            user.SetType(type);
            user.SetMonthlySalary(annualSalary.Value / 12);
            user.SetTags(tags);

            await Task.CompletedTask;
        }
    }
}