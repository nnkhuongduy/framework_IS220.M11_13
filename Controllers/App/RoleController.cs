using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using _99phantram.Entities;
using _99phantram.Helpers;
using _99phantram.Interfaces;
using _99phantram.Models;

namespace _99phantram.Controllers.App
{
  [Route("/api/app/roles")]
  [ApiController]
  [ServiceFilter(typeof(AppAuthorize))]
  public class RoleController : ControllerBase
  {
    private readonly IRoleService _roleService;
    private readonly JwtHolder _jwtHolder;

    public RoleController(IRoleService roleService, JwtHolder jwtHolder)
    {
      _roleService = roleService;
      _jwtHolder = jwtHolder;
    }

    [HttpGet("selectable")]
    public async Task<ActionResult<List<Role>>> GetSelectableRoles()
    {
      var user = _jwtHolder.User;

      return await _roleService.GetSelectableRoles(user);
    }
  }
}