using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cloud_Computing_TimGras_630259.Helper;
using DAL.Service;
using Domain;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Cloud_Computing_TimGras_630259
{
    public static class ProcessMortgages
    {
        [FunctionName("ProssesMortgages")]
        public static async Task Run([ServiceBusTrigger("testqueue", Connection = "AzureConnectionString")] string myQueueItem, ILogger log)
        {
            var users = System.Text.Json.JsonSerializer.Deserialize<List<User>>(myQueueItem);

            //save calculation
            var Helper = await CosmosHelper.InitCosmos("COSMOSMORTGAGECONTAINER");
            var containerName = Environment.GetEnvironmentVariable("COSMOSMORTGAGECONTAINER");

            var cosmosDbService = new CosmosMortgageDbService(Helper.Client, Helper.DBName, containerName);

            foreach (var user in users)
            {
                //calculate mortgage(what they can borrow)
                //calculation is fake need to find a real one
                var canLoan = (user.Income * 0.52) * 10;
                var mortgage = new Mortgage()
                {
                    Email = user.Email,
                    id = Guid.NewGuid(),
                    MortgageAmount = canLoan,
                };
                await cosmosDbService.AddAsync(mortgage);
            }
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        }
    }
}
