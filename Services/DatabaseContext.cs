using Microsoft.Extensions.Logging;
using _99phantram.Interfaces;
using MongoDB.Driver;

namespace _99phantram.Services
{
  public class DatabaseContext : IDatabaseContext
  {
    private readonly ILogger<DatabaseContext> _logger;
    private readonly IDatabaseContextOptions _options;
    private readonly string _baseUrl = "mongodb+srv://";
    private IMongoClient _client;
    public IMongoDatabase Database { get; }

    private void _Initialize()
    {
      string connectionString = _baseUrl + _options.Username + ":" + _options.Password + "@" + _options.ConnectionString + "/" + _options.DatabaseName + "?retryWrites=true&w=majority";

      MongoClientSettings settings = MongoClientSettings.FromConnectionString(connectionString);

      _logger.LogInformation("Successfully connecting to the MongoDB");

      _client = new MongoClient(settings);
    }

    public DatabaseContext(ILogger<DatabaseContext> logger, IDatabaseContextOptions options)
    {
      _logger = logger;
      _options = options;

      _Initialize();

      Database = _client.GetDatabase(_options.DatabaseName);
    }
  }
}
