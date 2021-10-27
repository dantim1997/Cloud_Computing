using DAL.EF;
using DAL.Interface;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class UserRepository : IUserRepository
    {
        private UserContext _UserContext;

        public UserRepository(UserContext userContext)
        {
            _UserContext = userContext;
        }

        public async Task<User> CreateBuyer(User user)
        {
            _UserContext.Add(user);
            await _UserContext.SaveChangesAsync();
            
            return user;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return _UserContext.Users;
        }
    }
}
