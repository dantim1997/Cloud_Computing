using DAL.EF;
using DAL.Interface;
using Domain;
using System.Collections.Generic;
using System.Linq;
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

        // create a buyer
        public async Task<User> CreateBuyer(User user)
        {
            _UserContext.Add<User>(user);
            await _UserContext.SaveChangesAsync();
            
            return user;
        }

        // edit a buyer
        public async Task<User> EditUser(User user)
        {
            _UserContext.Update(user);
            await _UserContext.SaveChangesAsync();

            return user;
        }

        // edit a list of users at the same time
        public async Task EditUsers(IEnumerable<User> users)
        {
            _UserContext.UpdateRange(users);
            await _UserContext.SaveChangesAsync();
        }

        // get buyer by Id
        public async Task<User> GetUserById(string user)
        {
            return _UserContext.Users.Find(user);
        }

        // get all the buyers
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var users = _UserContext.Users.ToList();
            return users;
        }
    }
}
