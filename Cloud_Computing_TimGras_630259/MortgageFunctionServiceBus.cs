using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ServiceLayer.Interface;
using System;
using System.Threading.Tasks;

namespace Cloud_Computing_TimGras_630259
{
    public class MortgageFunctionServiceBus
    {
        private readonly IMortgageService _MortgageService;
        private readonly ILogger<MortgageFunctionServiceBus> _Logger;

        //Service bus helps with handeling peak loads where a spike in messages might slow down the processing application.
        public MortgageFunctionServiceBus(ILogger<MortgageFunctionServiceBus> logger, IMortgageService mortgageService)
        {
            _Logger = logger;
            _MortgageService = mortgageService;
        }

        // start calculating the mortgages and upload the pdf
        [Function("CalculateMortgages")]
        public async Task CalculateMortgages([ServiceBusTrigger("testqueue", Connection = "AzureConnectionString")] string myQueueItem, FunctionContext context)
        {
            try
            {
                await _MortgageService.CreateMortgage(myQueueItem);
                _Logger.LogInformation($"{DateTime.Now} The Queue has been handled");
            }
            catch (Exception e)
            {
                _Logger.LogError("{Error}", e.Message);
                throw;
            }
        }

        // deletes the pdfs when they can no longer be visable
        [Function("SendMortgages")]
        public async Task SendMortgages([ServiceBusTrigger("sendfilequeue", Connection = "AzureConnectionString")] string myQueueItem, FunctionContext context)
        {
            try
            {
                await _MortgageService.SendEmailWithMortgages(myQueueItem);
                _Logger.LogInformation($"{DateTime.Now} The Queue has been handled");
            }
            catch (Exception e)
            {
                _Logger.LogError("{Error}", e.Message);
                throw;
            }
        }

        // deletes the pdfs when they can no longer be visable
        [Function("DeleteMortgages")]
        public async Task DeleteMortgages([ServiceBusTrigger("deletefilequeue", Connection = "AzureConnectionString")] string myQueueItem, FunctionContext context)
        {
            try
            {
                await _MortgageService.DeleteMortgage(myQueueItem);
                _Logger.LogInformation($"{DateTime.Now} The Queue has been handled");
            }
            catch (Exception e)
            {
                _Logger.LogError("{Error}", e.Message);
                throw;
            }
        }
    }
}
