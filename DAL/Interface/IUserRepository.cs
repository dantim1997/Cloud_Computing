using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface IUserRepository
    {
        Task<User> CreateBuyer(User user);
        Task<User> EditUser(User user);
        Task EditUsers(IEnumerable<User> users);
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserById(string user);
    }
}
