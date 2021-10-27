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
    public class MortgageRepository : IMortgageRepository
    {
        private MortgageContext _MortgageContext;

        public MortgageRepository(MortgageContext mortgageContext)
        {
            _MortgageContext = mortgageContext;
        }
        public async Task<bool> CreateMortgage(Mortgage mortgage)
        {
            _MortgageContext.Add(mortgage);
            await _MortgageContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Mortgage>> GetAllMortgages()
        {
            return _MortgageContext.Mortgages;
        }
    }
}
