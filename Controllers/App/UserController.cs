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

    public UserController(IUserService userService, IAuthService authService)
    {
      _userService = userService;
      _authService = authService;
    }

    [HttpGet]
    [TypeFilter(typeof(AppAuthorize))]
    public async Task<ActionResult<List<User>>> GetAllUsers()
    {
      return await DB.Find<User>().Match(_ => true).Sort(u => u.CreatedOn, MongoDB.Entities.Order.Descending).Project(user => user.Exclude("password")).ExecuteAsync();
    }

    [HttpGet("{id:length(24)}")]
    [TypeFilter(typeof(AppAuthorize))]
    public async Task<ActionResult<User>> getUser(string id)
    {
      var user = await DB.Find<User>().Match(user => user.ID == id).Project(user => user.Exclude("password")).ExecuteFirstAsync();

      if (user != null)
      {
        return user;
      }

      return NotFound(new HttpError(false, 404, "Không tìm thấy người dùng!"));
    }

    [HttpPost]
    [TypeFilter(typeof(AppAuthorize))]
    public async Task<ActionResult> CreateUser(PostUserBody body)
    {
      var role = await DB.Find<Role>().Match(role => role.ID.Equals(body.Role)).ExecuteFirstAsync();

      if (role == null)
      {
        return BadRequest(new HttpError(false, 404, "Không tìm thấy vai trò!"));
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
      user.Avatar = body.Avatar;
 
      await user.SaveAsync();

      return StatusCode(201);
    }

    [HttpPut("{id:length(24)}")]
    [TypeFilter(typeof(AppAuthorize))]
    public async Task<ActionResult<User>> UpdateUser(PutUserBody body, string id)
    {
      var user = await DB.Find<User>().Match(user => user.ID == id).ExecuteFirstAsync();

      if (user == null)
      {
        return NotFound(new HttpError(false, 404, "Không tìm thấy người dùng!"));
      }

      var role = await DB.Find<Role>().Match(role => role.ID.Equals(body.Role)).ExecuteFirstAsync();

      if (role == null)
      {
        return BadRequest(new HttpError(false, 404, "Không tìm thấy vai trò!"));
      }

      var newUser = await DB.UpdateAndGet<User>()
        .MatchID(id)
        .Modify(user => user.Email, body.Email)
        .Modify(user => user.Password, string.IsNullOrEmpty(body.Password) ? user.Password : _authService.EncryptPassword(body.Password))
        .Modify(user => user.FirstName, body.FirstName)
        .Modify(user => user.LastName, body.LastName)
        .Modify(user => user.Sex, body.Sex)
        .Modify(user => user.Address, body.Address)
        .Modify(user => user.PhoneNumber, body.PhoneNumber)
        .Modify(user => user.Role, role)
        .Modify(user => user.Status, body.Status)
        .Modify(user => user.Avatar, body.Avatar)
        .Project(user => user.Exclude("password"))
        .ExecuteAsync();

      return StatusCode(204);
    }

    [HttpDelete("{id:length(24)}")]
    [TypeFilter(typeof(AppAuthorize))]
    public async Task<ActionResult> DeleteUser(string id)
    {
      var user = await DB.Find<User>().Match(user => user.ID == id).ExecuteFirstAsync();

      if (user == null)
      {
        return NotFound(new HttpError(false, 404, "Không tìm thấy người dùng!"));
      }

      await DB.DeleteAsync<User>(id);

      return StatusCode(204);
    }
  }
}