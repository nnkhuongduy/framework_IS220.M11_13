using System.Collections.Generic;
using System.Threading.Tasks;

using _99phantram.Entities;

namespace _99phantram.Interfaces
{
  public interface IRoleService
  {
    Task<Role> GetRole(string id);
    Task<List<Role>> GetSelectableRoles(User user);
  }
}