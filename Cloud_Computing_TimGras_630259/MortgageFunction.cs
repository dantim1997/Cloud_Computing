using Cloud_Computing_TimGras_630259.Helper;
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
        private readonly IUserService _UserService;
        private readonly ILogger<MortgageFunction> _Logger;
        public MortgageFunction(ILogger<MortgageFunction> logger, IMortgageService mortgageService, IUserService userService)
        {
            _Logger = logger;
            _MortgageService = mortgageService;
            _UserService = userService;
        }

        // Creates batches of Buyers that need to be proccesed to get the mortgages
        // Will trigger on 24:00
        [Function("StartCalculateMortgages")]
        public async void StartCalculateMortgages([TimerTrigger("0 * * * 1 *")] MyInfo myTimer, FunctionContext context, ILogger log)
        {
            await _MortgageService.CreateMortgageQueue();
            _Logger.LogInformation($"{DateTime.Now} The Queue has been created");
        }

        //Servicebus helps with handeling peak loads where a spike in messages might slow down the processing application.
        [Function("CalculateMortgages")]
        public void CalculateMortgages([ServiceBusTrigger("ServiceBusName", Connection = "AzureConnectionString")] string myQueueItem, FunctionContext context)
        {
            _MortgageService.CreateMortgage(myQueueItem);
            _Logger.LogInformation($"{DateTime.Now} The Queue has been handled");
        }

        // sends mails to the buyers with thair mortgage calculated
        // Will trigger on 09:00
        [Function("StartMailSending")]
        public async Task StartMailSending([TimerTrigger("0 * * * 1 *")] MyInfo myTimer, ILogger log)
        {
            await _MortgageService.GetAllMortgages();
            _Logger.LogInformation($"{DateTime.Now} All mails have been send");
        }
    }
}
