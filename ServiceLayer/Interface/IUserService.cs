using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interface
{
    public interface IUserService
    {
        Task CreateBuyer(User user);
        Task<IEnumerable<User>> GetAllUsers();
    }
}
