using Microsoft.Extensions.Hosting;
using rLibrary.Interfaces;
using rLibrary.Services;
using SharpConfig;

namespace rLibrary
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<CosmosDbService>();
            builder.Services.AddSingleton<MongoDbService>();
            builder.Services.AddSingleton<IDataBaseProvider, DataBaseProvider>();
            builder.Services.AddScoped<IPublishService, PublishService>();

            var app = builder.Build();

            //Custom Initialization
            using (var scope = app.Services.CreateScope())
            {
                var dbProvider = scope.ServiceProvider.GetRequiredService<IDataBaseProvider>();
                var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

                string rConfigPath = config.GetValue<string>("ConfigurationPath");
                Configuration rLibraryConfig = Configuration.LoadFromFile(rConfigPath);

                dbProvider.LoadConfiguration(rLibraryConfig);
            }

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}