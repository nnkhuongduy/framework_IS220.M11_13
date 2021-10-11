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
    private IUserService _userService;

    public UserController(IUserService userService)
    {
      _userService = userService;
    }

    [HttpGet]
    [TypeFilter(typeof(AppAuthorize))]
    public ActionResult<List<User>> GetAllUsers()
    {
      return _userService.GetUsers((u) => true).SortByDescending(u => u.CreatedAt).ToList();
    }

    [HttpPost]
    [TypeFilter(typeof(AppAuthorize))]
    public ActionResult CreateUser([FromBody] PostUserBody body)
    {
      return StatusCode(201);
    }
  }
}