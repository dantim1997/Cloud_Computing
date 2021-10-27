using DAL.Interface;
using Domain;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Service
{
    public class CosmosMortgageDbService : ICosmosMortgageDbService
    {
        private Container _container;

        public CosmosMortgageDbService(
            CosmosClient cosmosDbClient,
            string databaseName,
            string containerName)
        {
            _container = cosmosDbClient.GetContainer(databaseName, containerName);
        }
        public async Task AddAsync(Mortgage item)
        {
            await _container.CreateItemAsync(item, new PartitionKey(item.id.ToString()));
        }

        public async Task DeleteAsync(string id)
        {
            await _container.DeleteItemAsync<Mortgage>(id, new PartitionKey(id));
        }

        public async Task<Mortgage> GetAsync(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<Mortgage>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException) //For handling item not found and other exceptions
            {
                return null;
            }
        }
        public async Task<IEnumerable<Mortgage>> GetMultipleAsync(string queryString)
        {
            var query = _container.GetItemQueryIterator<Mortgage>(new QueryDefinition(queryString));

            var results = new List<Mortgage>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task UpdateAsync(string id, Mortgage item)
        {
            await _container.UpsertItemAsync(item, new PartitionKey(id));
        }
    }
}
