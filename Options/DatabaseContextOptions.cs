using _99phantram.Interfaces;
using Microsoft.Extensions.Configuration;

namespace _99phantram.Options
{
  public class DatabaseContextOptions : IDatabaseContextOptions
  {
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string Username { get; }
    public string Password { get; }

    public DatabaseContextOptions(IConfiguration config)
    { 
      IConfiguration appsettings = config.GetSection(nameof(DatabaseContextOptions));

      Username = config["Database:Username"];
      Password = config["Database:Password"];
      ConnectionString = appsettings["ConnectionString"];
      DatabaseName = appsettings["DatabaseName"];
    }
  }
}
