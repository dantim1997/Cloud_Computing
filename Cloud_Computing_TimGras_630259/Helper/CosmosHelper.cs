using Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Computing_TimGras_630259.Helper
{
    public static class CosmosHelper
    {
        public async static Task<CosmosConnector> InitCosmos(string container)
        {
            var databaseName = Environment.GetEnvironmentVariable("COSMOSDB");
            var containerName = Environment.GetEnvironmentVariable(container);
            var account = Environment.GetEnvironmentVariable("Account");
            var key = Environment.GetEnvironmentVariable("COSMOSDBKEY");

            var client = new Microsoft.Azure.Cosmos.CosmosClient(account, key);
            var database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");

            return new CosmosConnector()
            {
                Client = client,
                DBName = databaseName
            };
        }
    }
}
