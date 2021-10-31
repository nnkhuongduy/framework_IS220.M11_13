using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using _99phantram.Entities;
using _99phantram.Helpers;
using _99phantram.Interfaces;

namespace _99phantram.Controllers.App
{
  [Route("/api/app/roles")]
  [ApiController]
  public class RoleController : ControllerBase
  {
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
      _roleService = roleService;
    }

    [HttpGet("selectable")]
    [TypeFilter(typeof(AppAuthorize))]
    public async Task<ActionResult<List<Role>>> GetSelectableRoles()
    {
      var user = (User)HttpContext.Items["User"];

      return await _roleService.GetSelectableRoles(user);
    }
  }
}