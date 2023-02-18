using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos;
using rLibrary.Entities.Enums;
using rLibrary.Entities.Objects.DTOs.DbProvider;
using rLibrary.Interfaces;

namespace rLibrary.Services
{
    public class CosmosDbService : IDatabaseService
    {
        private readonly string databaseName;
        private CosmosClient client;

        public virtual async Task Init(object parameters)
        {
            CosmosDbVars vars = (CosmosDbVars)parameters;
        }

        public async Task CreateCollection(DataBaseCollections collection)
        {
            DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            await database.Database.CreateContainerIfNotExistsAsync(collection.ToString(), "/id");
        }

        public async Task<List<T>> GetDocuments<T>(DataBaseCollections collection, string query)
        {
            Container container = client.GetContainer(databaseName, collection.ToString());
            var rawResult = container.GetItemQueryIterator<T>(new QueryDefinition(query));
            List<T> results = new List<T>();
            while (rawResult.HasMoreResults)
            {
                var response = await rawResult.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }

        public async Task InsertDocument<T>(DataBaseCollections collection, string id, T Document)
        {
            Container container = client.GetContainer(databaseName, collection.ToString());
            await container.CreateItemAsync(Document, new PartitionKey(id));
        }

        public async Task UpdateDocument<T>(DataBaseCollections collection, string id, T Document)
        {
            Container container = client.GetContainer(databaseName, collection.ToString());
            await container.UpsertItemAsync(Document, new PartitionKey(id));
        }

        public async Task DeleteDocument<T>(DataBaseCollections collection, string id)
        {
            Container container = client.GetContainer(databaseName, collection.ToString());
            await container.DeleteItemAsync<T>(id, new PartitionKey(id));
        }
    }
}
