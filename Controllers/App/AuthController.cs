using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using _99phantram.Models;
using _99phantram.Entities;
using _99phantram.Helpers;
using _99phantram.Interfaces;
using System.Threading.Tasks;
using MongoDB.Entities;

namespace _99phantram.Controllers.App
{
  [Route("/api/app/auth")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private IUserService _userService;
    private JwtHolder _jwtHolder;
    private IAuthService _authService;

    public AuthController(IUserService userService, IAuthService authService, JwtHolder jwtHolder)
    {
      _userService = userService;
      _jwtHolder = jwtHolder;
      _authService = authService;
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<AuthResponse>> Login(AuthRequest request)
    {
      try
      {
        return await _authService.LoginEmployee(request);
      }
      catch (HttpError error)
      {
        return StatusCode(error.Code, error);
      }
    }

    [HttpGet]
    [ServiceFilter(typeof(AppAuthorize))]
    public ActionResult<User> Authenticate()
    {
      return _jwtHolder.User;
    }
  }
}