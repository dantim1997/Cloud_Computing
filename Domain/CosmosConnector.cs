using Microsoft.Azure.Cosmos;

namespace Domain
{
    public class CosmosConnector
    {
        public string DBName { get; set; }
        public CosmosClient Client { get; set; }
    }
}
