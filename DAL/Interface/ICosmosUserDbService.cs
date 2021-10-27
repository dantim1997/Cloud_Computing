using Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface ICosmosUserDbService
    {
        Task AddAsync(User item);
        Task DeleteAsync(string id);
        Task<User> GetAsync(string id);
        Task<IEnumerable<User>> GetMultipleAsync(string queryString);
        Task UpdateAsync(string id, User item);
    }
}
