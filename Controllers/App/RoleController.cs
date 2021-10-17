using System.Collections.Generic;
using System.Threading.Tasks;
using _99phantram.Entities;
using _99phantram.Helpers;
using _99phantram.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Entities;
using MongoDB.Driver;

namespace _99phantram.Controllers.App
{
  [Route("/api/app/roles")]
  [ApiController]
  public class RoleController : ControllerBase
  {
    private readonly IRoleService _roleService;
    private readonly ILogger<RoleController> _logger;

    public RoleController(IRoleService roleService, ILogger<RoleController> logger)
    {
      _roleService = roleService;
      _logger = logger;
    }

    [HttpGet("selectable")]
    [TypeFilter(typeof(AppAuthorize))]
    public async Task<ActionResult<List<Role>>> GetSelectableRoles()
    {
      var user = (User)HttpContext.Items["User"];

      return await DB.Find<Role>().Match(role => role.In("_id", user.Role.SelectableRoles)).Project(role => role.Exclude("selectable_roles")).ExecuteAsync();
    }
  }
}