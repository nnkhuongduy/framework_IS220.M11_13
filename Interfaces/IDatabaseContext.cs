using MongoDB.Driver;

namespace _99phantram.Interfaces
{
  public interface IDatabaseContext
  {
    IMongoDatabase Database { get; }
  }
}