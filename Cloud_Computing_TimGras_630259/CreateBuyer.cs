using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text.Json;
using Domain;
using DAL.Service;
using Cloud_Computing_TimGras_630259.Helper;

namespace Cloud_Computing_TimGras_630259
{
    public static class CreateBuyer
    {
        [FunctionName("CreateBuyer")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            //get user
            var _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, AllowTrailingCommas = true };
            string name = req.Query["name"];
            var body = req.Body;

            //make user class
            var getInfo = await System.Text.Json.JsonSerializer.DeserializeAsync<User>(body, _options);

            //create user in db
            var Helper = await CosmosHelper.InitCosmos("COSMOSUSERCONTAINER");
            var containerName = Environment.GetEnvironmentVariable("COSMOSUSERCONTAINER");

            var cosmosDbService = new CosmosUserDbService(Helper.Client, Helper.DBName, containerName);


            getInfo.id = Guid.NewGuid();
            await cosmosDbService.AddAsync(getInfo);

            //TODO
            //save user for create

            string responseMessage = string.IsNullOrEmpty(getInfo.Name)
                ? "Name was not given."
                : $"Account for user: {name}. Has been created.";

            return new OkObjectResult(responseMessage);
        }
    }
}
