using System;
using Cloud_Computing_TimGras_630259.Helper;
using DAL.Service;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Cloud_Computing_TimGras_630259
{
    public static class DeleteMortgage
    {
        [FunctionName("DeleteMortgage")]
        public static async System.Threading.Tasks.Task RunAsync([TimerTrigger("0 * * * * *")]TimerInfo myTimer, ILogger log)
        {
            var Helper = await CosmosHelper.InitCosmos("COSMOSMORTGAGECONTAINER");
            var containerName = Environment.GetEnvironmentVariable("COSMOSMORTGAGECONTAINER");

            var cosmosDbService = new CosmosMortgageDbService(Helper.Client, Helper.DBName, containerName);
            var query = $"SELECT * FROM c";
            var mortgages = await cosmosDbService.GetMultipleAsync(query);
            foreach (var mortgage in mortgages)
            {
                cosmosDbService.DeleteAsync(mortgage.id.ToString());
            }

            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
