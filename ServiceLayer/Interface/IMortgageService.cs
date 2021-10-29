using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interface
{
    public interface IMortgageService
    {
        Task CreateMortgage(string mortgage);
        Task CreateMortgageQueue();
        Task DeleteMortgage(string queueString);
        Task DeleteMortgageQueue();
        Task SendEmailWithMortgages(string queueString);
        Task SendMortgageQueue();
    }
}
