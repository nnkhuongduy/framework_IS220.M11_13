using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using _99phantram.Entities;
using _99phantram.Helpers;
using _99phantram.Interfaces;
using _99phantram.Models;

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
      return await _userService.GetAllUsers(false);
    }

    [HttpGet("{id:length(24)}")]
    [TypeFilter(typeof(AppAuthorize))]
    public async Task<ActionResult<User>> getUser(string id)
    {
      try
      {
        var user = await _userService.GetUser(id, false);

        return user;
      }
      catch (HttpError error)
      {
        return NotFound(error);
      }
    }

    [HttpPost]
    [TypeFilter(typeof(AppAuthorize))]
    public async Task<ActionResult> CreateUser(PostUserBody body)
    {
      await _userService.CreateUser(body);

      return StatusCode(201);
    }

    [HttpPut("{id:length(24)}")]
    [TypeFilter(typeof(AppAuthorize))]
    public async Task<ActionResult<User>> UpdateUser(PutUserBody body, string id)
    {
      try
      {
        await _userService.UpdateUser(id, body);

        return StatusCode(204);
      }
      catch (HttpError error)
      {
        return NotFound(error);
      }
    }

    [HttpDelete("{id:length(24)}")]
    [TypeFilter(typeof(AppAuthorize))]
    public async Task<ActionResult> DeleteUser(string id)
    {
      try
      {
        await _userService.DeleteUser(id);

        return StatusCode(204);
      }
      catch (HttpError error)
      {
        return NotFound(error);
      }
    }
  }
}