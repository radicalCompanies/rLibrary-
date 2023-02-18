using Moq;
using rLibrary.Entities.Objects.DTOs.DbProvider;
using rLibrary.Interfaces;
using rLibrary.Services;
using SharpConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rLibrary.Tests.Services
{
    [TestClass]
    public class DataBaseProviderTests
    {
        IDataBaseProvider dbProvider;

        Mock<CosmosDbService> mockCosmosDb = new Mock<CosmosDbService>();
        Mock<MongoDbService> mockMongoDb = new Mock<MongoDbService>();

        private void SeedTestingContext() {
            dbProvider = new DataBaseProvider(mockCosmosDb.Object, mockMongoDb.Object);
        }

        [TestMethod]
        public void getCurrentDbService_Cosmos_Ok() {
            //Arrange 
            SeedTestingContext();

            Configuration myConfig = new Configuration();
            myConfig["General"]["DataBaseType"].StringValue = "CosmosDB";
            
            myConfig["CosmosDB"]["DatabaseName"].StringValue = "SomeDbName";
            myConfig["CosmosDB"]["Key"].StringValue = "SomeKey";
            myConfig["CosmosDB"]["Account"].StringValue = "SomeAccountUrl";

            mockCosmosDb.Setup(x => x.Init(It.IsAny<object>()));

            //Act
            dbProvider.LoadConfiguration(myConfig);

            //Assert
            CosmosDbService assert = (CosmosDbService)dbProvider.currentDataBaseService;

            mockCosmosDb.Verify(x => x.Init(It.Is<object>(o => 
            o.GetType() == typeof(CosmosDbVars)
            && ((CosmosDbVars)o).Account == "SomeAccountUrl"
            && ((CosmosDbVars)o).Key == "SomeKey"
            && ((CosmosDbVars)o).DatabaseName == "SomeDbName")), Times.Once);

        }

        [TestMethod]
        public void getCurrentDbService_Mongo_Ok() {
            //Arrange 
            SeedTestingContext();

            Configuration myConfig = new Configuration();
            myConfig["General"]["DataBaseType"].StringValue = "MongoDB";

            myConfig["MongoDB"]["ConnectionString"].StringValue = "SomeMongoDbConnectionString";
            myConfig["MongoDB"]["DatabaseName"].StringValue = "TheDataBaseName";

            mockMongoDb.Setup(x => x.Init(It.IsAny<object>()));

            //Act
            dbProvider.LoadConfiguration(myConfig);

            //Assert
            MongoDbService assert = (MongoDbService)dbProvider.currentDataBaseService;

            mockMongoDb.Verify(x => x.Init(It.Is<object>(o => 
            o.GetType() == typeof(MongoDbVars)
            && ((MongoDbVars)o).ConnectionString == "SomeMongoDbConnectionString"
            && ((MongoDbVars)o).DatabaseName == "TheDataBaseName")), Times.Once);
        }
    }
}
