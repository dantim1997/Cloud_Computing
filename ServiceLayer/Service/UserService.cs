using DAL.Interface;
using Domain;
using Microsoft.Extensions.Logging;
using ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _UserRepository;
        private readonly ILogger<UserService> _Logger;
        public UserService(ILogger<UserService> logger, IUserRepository userRepository)
        {
            _UserRepository = userRepository;
            _Logger = logger;
        }
        public async Task CreateBuyer(User user)
        {
            var createUser = await _UserRepository.CreateBuyer(user);
            _Logger.LogInformation($"{createUser.Name} has been created as buyer");
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _UserRepository.GetAllUsers();
        }
    }
}
