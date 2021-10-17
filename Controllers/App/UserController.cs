using System.Collections.Generic;
using _99phantram.Entities;
using _99phantram.Helpers;
using _99phantram.Interfaces;
using _99phantram.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace _99phantram.Controllers.App
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
    public ActionResult<List<User>> GetAllUsers()
    {
      return _userService.GetUsers(Builders<User>.Filter.Empty).SortByDescending(u => u.CreatedAt).Project<User>(Builders<User>.Projection.Exclude(u => u.Password)).ToList();
    }

    [HttpGet("{id:length(24)}")]
    [TypeFilter(typeof(AppAuthorize))]
    public ActionResult<User> getUser(string id)
    {
      var user = _userService.GetUser(u => u.Id == id).Project<User>(Builders<User>.Projection.Exclude(u => u.Password)).FirstOrDefault();

      if (user != null)
      {
        return user;
      }

      return NotFound(new HttpError(false, 404, "User not found"));
    }

    [HttpPost]
    [TypeFilter(typeof(AppAuthorize))]
    public ActionResult CreateUser(PostUserBody body)
    {
      var role = _roleService.GetRole((r) => r.Id.Equals(body.Role)).FirstOrDefault();

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
      user.Avatar = body.Avatar;

      _userService.CreateUser(user);

      return StatusCode(201);
    }
  }
}