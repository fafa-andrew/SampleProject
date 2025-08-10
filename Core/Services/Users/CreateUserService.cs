using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BusinessEntities;
using Common;
using Core.Factories;
using Data.Repositories;

namespace Core.Services.Users
{
    [AutoRegister]
    public class CreateUserService : ICreateUserService
    {
        private readonly IUpdateUserService _updateUserService;
        private readonly IIdObjectFactory<User> _userFactory;
        private readonly IUserRepository _userRepository;

        public CreateUserService(IIdObjectFactory<User> userFactory, IUserRepository userRepository, IUpdateUserService updateUserService)
        {
            _userFactory = userFactory;
            _userRepository = userRepository;
            _updateUserService = updateUserService;
        }

        public async Task<User> CreateAsync
            (
            Guid id, 
            string name, 
            string email, 
            int age,
            UserTypes type,
            decimal? annualSalary, 
            IEnumerable<string> tags,
            CancellationToken ct
            )
        {
            var user = _userFactory.Create(id);
            await _updateUserService.UpdateAsync(user, name, email, age, type, annualSalary, tags, ct);
            await _userRepository.StoreAsync(user, ct);

            return user;
        }
    }
}