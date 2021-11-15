using System.Collections.Generic;
using System.Threading.Tasks;

using _99phantram.Entities;
using _99phantram.Interfaces;
using _99phantram.Models;
using MongoDB.Entities;

namespace _99phantram.Services
{
  public class RoleService : IRoleService
  {
    public async Task<Role> GetRole(string id)
    {
      var role = await DB.Find<Role>().MatchID(id).ExecuteFirstAsync();

      if (role == null)
      {
        throw new HttpError(false, 404, "Không tìm thấy vai trò!");
      }

      return role;
    }

    public async Task<List<Role>> GetSelectableRoles(User user)
    {
      return await DB.Find<Role>().Match(role => role.In("_id", user.Role.SelectableRoles)).Project(role => role.Exclude("selectable_roles")).ExecuteAsync();
    }
  }
}