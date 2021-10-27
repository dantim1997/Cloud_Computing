using Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface ICosmosMortgageDbService
    {
        Task AddAsync(Mortgage item);
        Task DeleteAsync(string id);
        Task<Mortgage> GetAsync(string id);
        Task<IEnumerable<Mortgage>> GetMultipleAsync(string queryString);
        Task UpdateAsync(string id, Mortgage item);
    }
}
