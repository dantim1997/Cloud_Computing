using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Cloud_Computing_TimGras_630259.Helper;
using DAL.Service;

namespace Cloud_Computing_TimGras_630259
{
    public static class ViewMortgage
    {
        [FunctionName("ViewMortgage")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            string name = req.Query["Mortgage"];
            string responseMessage = "No Mortgage found";

            var Helper = await CosmosHelper.InitCosmos("COSMOSMORTGAGECONTAINER");
            var containerName = Environment.GetEnvironmentVariable("COSMOSMORTGAGECONTAINER");

            var cosmosDbService = new CosmosMortgageDbService(Helper.Client, Helper.DBName, containerName);
            var mortgage = await cosmosDbService.GetAsync(name);
            if (mortgage != null)
            {
                log.LogInformation("Found mortgage.");
                responseMessage = $"Your mortgage is ${mortgage.MortgageAmount}";
            }
            else
                log.LogInformation("No mortgage was found.");

            return new OkObjectResult(responseMessage);
        }
    }
}
