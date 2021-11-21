using System.Collections.Generic;
using System.Threading.Tasks;
using _99phantram.Entities;
using _99phantram.Models;

namespace _99phantram.Interfaces
{
    public interface ILocationService
  {
    Task<List<Location>> GetAllLocations();
    Task<Location> GetLocation(string id);
    Task<Location> CreateLocation(LocationBody body);
    Task<Location> UpdateLocation(LocationBody body, string id);
    Task<Location> ArchiveLocation(Location location);
    Task DeleteLocation(string id);
  }
}