using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface IMortgageRepository
    {
        Task<bool> CreateMortgage(Mortgage mortgage);
        Task<IEnumerable<Mortgage>> GetAllMortgages();
    }
}
