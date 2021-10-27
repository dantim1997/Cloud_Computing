using Azure.Messaging.ServiceBus;
using Cloud_Computing_TimGras_630259.Helper;
using DAL.Service;
using Domain;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Computing_TimGras_630259
{
    public static class CalculateMortgages
    {

        [FunctionName("CalculateMortgages")]
        public static async Task Run([TimerTrigger("0 * * * 1 *")] TimerInfo myTimer, ILogger log)
        {

            //create user in db
            var Helper = await CosmosHelper.InitCosmos("COSMOSUSERCONTAINER");
            var containerName = Environment.GetEnvironmentVariable("COSMOSUSERCONTAINER");

            var cosmosDbService = new CosmosUserDbService(Helper.Client, Helper.DBName, containerName);

            var query = $"SELECT * FROM c";
            var users = await cosmosDbService.GetMultipleAsync(query);

            string ServiceBusConnectString = Environment.GetEnvironmentVariable("AzureConnectionString");
            string QueueName = Environment.GetEnvironmentVariable("ServiceBusName");
            if (!string.IsNullOrEmpty(QueueName))
            {
                IQueueClient client = new QueueClient(ServiceBusConnectString, QueueName);
                //will take chunks and give that to the service bus(improve speed)
                var ChuckListOfUsers = SpliterHelper.Split(users.ToList());
                //send user mortgage info to queue to be calculated(what they can borrow)
                foreach (var Chunkusers in ChuckListOfUsers)
                {
                    var messageBody = JsonConvert.SerializeObject(Chunkusers);
                    var message = new Message(Encoding.UTF8.GetBytes(messageBody));
                    await client.SendAsync(message);
                }
                log.LogInformation($"{DateTime.Now} Mortgage has been calculated for {users.ToList().Count} users.");
            }
        }
        
        
    }
}
