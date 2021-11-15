using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Entities;

namespace _99phantram.Services
{
  class MongoDBContext
  {
    public static void InitMongoDB(IConfiguration configuration, ILogger logger)
    {
      IConfiguration appsettings = configuration.GetSection("DatabaseContextOptions");
      var connectionString = "mongodb+srv://" + configuration["Database:Username"] + ":" + configuration["Database:Password"] + "@" + appsettings["ConnectionString"] + "/" + "?retryWrites=true&w=majority";

      Task.Run(async () =>
      {
        await DB.InitAsync(appsettings["DatabaseName"], MongoClientSettings.FromConnectionString(connectionString));
      }).GetAwaiter().GetResult();

      logger.LogInformation("Successfully connecting to the MongoDB");
    }
  }
}