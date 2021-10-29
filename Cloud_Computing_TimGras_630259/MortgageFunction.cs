using Domain;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServiceLayer.Interface;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Computing_TimGras_630259
{
    public class MortgageFunction
    {
        private readonly IMortgageService _MortgageService;
        private readonly ILogger<MortgageFunction> _Logger;
        public MortgageFunction(ILogger<MortgageFunction> logger, IMortgageService mortgageService)
        {
            _Logger = logger;
            _MortgageService = mortgageService;
        }

        // Creates batches of Buyers that need to be proccesed to get the mortgages
        // Will trigger on 24:00
        [Function("StartCalculateMortgages")]
        public async void StartCalculateMortgages([TimerTrigger("0 0 0 * * *")] MyInfo myTimer, FunctionContext context, ILogger log)
        {
            await _MortgageService.CreateMortgageQueue();
            _Logger.LogInformation($"{DateTime.Now} The Queue has been created");
        }

        // sends mails to the buyers with thair mortgage calculated
        // Will trigger on 09:00
        [Function("StartMailSending")]
        public async Task StartMailSending([TimerTrigger("0 0 9 * * *")] MyInfo myTimer, ILogger log)
        {
            try
            {
                await _MortgageService.GetAllMortgages();
                _Logger.LogInformation($"{DateTime.Now} All mails have been send");
            }
            catch (Exception e)
            {
                _Logger.LogError("{Error}", e.Message);
                throw;
            }
        }

        // sends mails to the buyers with thair mortgage calculated
        // Will trigger on 23:00
        [Function("DeleteMortgage")]
        public async Task DeleteMortgage([TimerTrigger("0 0 23 * * *")] MyInfo myTimer, ILogger log)
        {
            await _MortgageService.DeleteMortgageQueue();
            _Logger.LogInformation($"{DateTime.Now} All Mortgages have been deleted");
        }
    }
}
