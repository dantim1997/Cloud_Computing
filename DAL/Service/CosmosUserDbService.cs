using DAL.Interface;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Service
{
    public class CosmosUserDbService : ICosmosUserDbService
    {
        private Container _container;

        public CosmosUserDbService(
            CosmosClient cosmosDbClient,
            string databaseName,
            string containerName)
        {
            _container = cosmosDbClient.GetContainer(databaseName, containerName);
        }
        public async Task AddAsync(Domain.User item)
        {
            await _container.CreateItemAsync(item, new PartitionKey(item.id.ToString()));
        }

        public async Task DeleteAsync(string id)
        {
            await _container.DeleteItemAsync<User>(id, new PartitionKey(id));
        }

        public async Task<Domain.User> GetAsync(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<Domain.User>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException) //For handling item not found and other exceptions
            {
                return null;
            }
        }
        public async Task<IEnumerable<Domain.User>> GetMultipleAsync(string queryString)
        {
            var query = _container.GetItemQueryIterator<Domain.User>(new QueryDefinition(queryString));

            var results = new List<Domain.User>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task UpdateAsync(string id, Domain.User item)
        {
            await _container.UpsertItemAsync(item, new PartitionKey(id));
        }
    }
}
