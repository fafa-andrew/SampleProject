using BusinessEntities;
using Common;
using Core.Services.Users.Contracts;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Services.Users
{
    [AutoRegister(AutoRegisterTypes.Singleton)]
    public class UpdateUserService : IUpdateUserService
    {
        public async Task UpdateAsync(
            User user, 
            string name, 
            string email,
            int age,
            UserTypes type,
            decimal? annualSalary,
            IEnumerable<string> tags,
            CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            decimal? monthly = annualSalary.HasValue
                ? decimal.Round(annualSalary.Value / 12m, 2, MidpointRounding.AwayFromZero)
                : (decimal?)null;

            user.SetEmail(email);
            user.SetName(name);
            user.SetType(type);
            user.SetMonthlySalary(monthly);
            user.SetAge(age);
            user.SetTags(tags);

            await Task.CompletedTask;
        }
    }
}