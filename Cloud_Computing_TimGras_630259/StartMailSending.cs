using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Cloud_Computing_TimGras_630259.Helper;
using DAL.Service;
using Domain;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Cloud_Computing_TimGras_630259
{
    public static class StartMailSending
    {

        [FunctionName("StartMailSending")]
        public static async Task Run([TimerTrigger("0 * * * 1 *")] TimerInfo myTimer, ILogger log)
        {
            var Helper = await CosmosHelper.InitCosmos("COSMOSMORTGAGECONTAINER");
            var containerName = Environment.GetEnvironmentVariable("COSMOSMORTGAGECONTAINER");

            var cosmosDbService = new CosmosMortgageDbService(Helper.Client, Helper.DBName, containerName);
            var query = $"SELECT * FROM c";
            var mortgages = await cosmosDbService.GetMultipleAsync(query);

            //send all mortgages
            foreach (var mortgage in mortgages)
            {
                string hostLink = "http://localhost:7071/api/";
                var link = $"{hostLink}ViewMortgage?Mortgage={mortgage.id}";
                SendMail(mortgage.Email, link);
            }

            log.LogInformation($"Send all mails to the Users: {DateTime.Now}");
        }

        public static void SendMail(string email, string body)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(Environment.GetEnvironmentVariable("EmailUsername"), Environment.GetEnvironmentVariable("EmailPassword")),
                EnableSsl = true,
            };

            smtpClient.Send("we@test.nl", email, "See your mortgage", body);
        }
    }
}
