using System.Collections.Generic;
using System.Threading.Tasks;
using _99phantram.Entities;
using _99phantram.Helpers;
using _99phantram.Interfaces;
using _99phantram.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace _99phantram.Controllers.Apps
{
  [Route("/api/app/users")]
  [ApiController]
  public class UserController : ControllerBase
  {
    private readonly IUserService _userService;
    private readonly IAuthService _authService;
    private readonly IRoleService _roleService;

    public UserController(IUserService userService, IAuthService authService, IRoleService roleService)
    {
      _userService = userService;
      _authService = authService;
      _roleService = roleService;
    }

    [HttpGet]
    [TypeFilter(typeof(AppAuthorize))]
    public async Task<ActionResult<List<User>>> GetAllUsers()
    {
      return await DB.Find<User>().Match(_ => true).Sort(u => u.CreatedOn, Order.Descending).ExecuteAsync();
    }

    [HttpPost]
    [TypeFilter(typeof(AppAuthorize))]
    public async Task<ActionResult> CreateUser(PostUserBody body)
    {
      var role = await DB.Find<Role>().Match(role => role.ID.Equals(body.Role)).ExecuteFirstAsync();

      if (role == null)
      {
        return BadRequest(new HttpError(false, 400, "Role not found!"));
      }

      Entities.User user = new Entities.User();

      user.Email = body.Email;
      user.Password = _authService.EncryptPassword(body.Password);
      user.FirstName = body.FirstName;
      user.LastName = body.LastName;
      user.Sex = body.Sex;
      user.Address = body.Address;
      user.PhoneNumber = body.PhoneNumber;
      user.Role = role;
      user.Status = body.Status;
      user.Oauth = false;
      user.OauthProvider = OAuthProvider.None;
      user.Avatar = null;

      await user.SaveAsync();

      return StatusCode(201);
    }
  }
}