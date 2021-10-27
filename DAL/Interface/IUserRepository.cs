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
        Task<IEnumerable<User>> GetAllUsers();
    }
}
