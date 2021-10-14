using System.Collections.Generic;
using _99phantram.Entities;
using _99phantram.Helpers;
using _99phantram.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
    public ActionResult<List<Role>> GetSelectableRoles()
    {
      var user = (User)HttpContext.Items["User"];
      var query = Builders<Role>.Filter.In("Id", user.Role.SelectableRoles);
      var projection = Builders<Role>.Projection.Exclude(r => r.SelectableRoles);
      
      return _roleService.GetRoles(query).Project<Role>(projection).ToList<Role>();
    }
  }
}