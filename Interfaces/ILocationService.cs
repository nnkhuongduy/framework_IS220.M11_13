using System.Threading.Tasks;

using _99phantram.Entities;
using _99phantram.Models;

namespace _99phantram.Interfaces
{
    public interface ILocationService
  {
    Task<Location> GetLocation(string id);
    Task<Location> CreateLocation(LocationBody body);
    Task<Location> UpdateLocation(string id, LocationBody body);
    Task<Location> ArchiveLocation(Location location);
    Task DeleteLocation(string id);
  }
}