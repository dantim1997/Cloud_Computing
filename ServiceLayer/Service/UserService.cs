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

        // Create buyer
        public async Task CreateBuyer(User user)
        {
            user.id = Guid.NewGuid().ToString();
            var createUser = await _UserRepository.CreateBuyer(user);
            _Logger.LogInformation($"{createUser.Name} has been created as buyer");
        }

        // Get a buyer based on the id
        public async Task<User> GetUserById(string userid)
        {
            return await _UserRepository.GetUserById(userid);
        }

        // Edit a buyer
        public async Task<User> EditUser(User user)
        {
            return await _UserRepository.EditUser(user);
        }

        // edit multiple buyers on the same time
        public async Task EditUsers(IEnumerable<User> users)
        {
            await _UserRepository.EditUsers(users);
        }

        // get all the buyers
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _UserRepository.GetAllUsers();
        }
    }
}
