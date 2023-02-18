using rLibrary.Entities.Enums;
using rLibrary.Interfaces;

namespace rLibrary.Services
{
    public class MongoDbService : IDatabaseService
    {
        public virtual Task Init(object parameters)
        {
            throw new NotImplementedException();
        }

        public Task CreateCollection(DataBaseCollections collection)
        {
            throw new NotImplementedException();
        }

        public Task DeleteDocument<T>(DataBaseCollections collection, string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> GetDocuments<T>(DataBaseCollections collection, string query)
        {
            throw new NotImplementedException();
        }

        public Task InsertDocument<T>(DataBaseCollections collection, string id, T Document)
        {
            throw new NotImplementedException();
        }

        public Task UpdateDocument<T>(DataBaseCollections collection, string id, T Document)
        {
            throw new NotImplementedException();
        }
    }
}
