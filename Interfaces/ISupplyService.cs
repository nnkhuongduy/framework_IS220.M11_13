using System.Collections.Generic;
using System.Threading.Tasks;

using _99phantram.Entities;
using _99phantram.Models;

namespace _99phantram.Interfaces
{
  public interface ISupplyService {
    Task<Supply> CreateSupply(User owner, ClientPostSupply body);
    Task<List<Supply>> GetAllSupplies();
    Task<List<Supply>> GetActiveSupplies(SupplyQueryFilter filter);
    Task<Supply> GetSupply(string id);
    Task<Supply> UpdateSupply(string id, PutSupply body);
    Task DeleteSupply(string id);
    Task<List<Supply>> GetOwnSupplies(User user);
  }
}
