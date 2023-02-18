using rLibrary.Entities.Objects.DTOs.DbProvider;
using rLibrary.Interfaces;
using SharpConfig;

namespace rLibrary.Services
{
    public class DataBaseProvider : IDataBaseProvider
    {
        private readonly CosmosDbService cosmosDb;
        private readonly MongoDbService mongoDb;

        public IDatabaseService currentDataBaseService { get; private set; }

        public DataBaseProvider(CosmosDbService cosmosDb, MongoDbService mongoDb)
        {
            this.cosmosDb = cosmosDb;
            this.mongoDb = mongoDb;
        }

        public void LoadConfiguration(Configuration rLibraryConfig)
        {
            string dbType = rLibraryConfig["General"]["DataBaseType"].StringValue;
            switch (dbType) {
                case "CosmosDB":
                    currentDataBaseService = cosmosDb;
                    CosmosDbVars cosmosVars = new CosmosDbVars
                    {
                        Account = rLibraryConfig["CosmosDB"]["Account"].StringValue,
                        Key = rLibraryConfig["CosmosDB"]["Key"].StringValue,
                        DatabaseName = rLibraryConfig["CosmosDB"]["DatabaseName"].StringValue,
                    };
                    currentDataBaseService.Init(cosmosVars).GetAwaiter().GetResult();
                    break;

                case "MongoDB":
                    currentDataBaseService = mongoDb;
                    MongoDbVars mongoVars = new MongoDbVars
                    {
                        ConnectionString = rLibraryConfig["MongoDB"]["ConnectionString"].StringValue,
                        DatabaseName = rLibraryConfig["MongoDB"]["DatabaseName"].StringValue,
                    };
                    currentDataBaseService.Init(mongoVars).GetAwaiter().GetResult();
                    break;
            }
        }
    }
}
