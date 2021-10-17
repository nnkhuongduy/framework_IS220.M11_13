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
    private ILogger _logger;
    private IAuthService _authService;

    public AuthController(IUserService userService, IAuthService authService, ILogger<AuthController> logger)
    {
      _userService = userService;
      _logger = logger;
      _authService = authService;
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<AuthResponse>> Login(AuthRequest request)
    {
      var employee = await DB.Find<User>().Match(e => e.Email == request.Email).ExecuteFirstAsync();

      if (employee == null)
      {
        return NotFound(new HttpError(false, 404, "Không tìm thấy nhân viên!"));
      }

      if (!_authService.VerifyPassword(request.Password, employee.Password))
      {
        return BadRequest(new HttpError(false, 400, "Mật khẩu không đúng!"));
      }

      var authResponse = _authService.Authenticate(employee, request.Remember == "true");

      return authResponse;
    }

    [HttpGet]
    [TypeFilter(typeof(AppAuthorize))]
    public ActionResult<User> Authenticate()
    {
      return (User)HttpContext.Items["User"];
    }
  }
}