using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using _99phantram.Entities;
using _99phantram.Helpers;
using _99phantram.Interfaces;
using _99phantram.Models;

namespace _99phantram.Controllers.Apps
{
  [Route("/api/auth")]
  [ApiController]
  public class ClientAuthController : ControllerBase
  {
    private readonly IUserService _userService;
    private readonly IAuthService _authService;
    private JwtHolder _jwtHolder;

    public ClientAuthController(IUserService userService, IAuthService authService, JwtHolder jwtHolder)
    {
      _userService = userService;
      _authService = authService;
      _jwtHolder = jwtHolder;
    }

    [HttpPost("registration")]
    public async Task<ActionResult> Registration(UserRegistrationBody body)
    {
      try
      {
        await _authService.Register(body);

        return StatusCode(201);
      }
      catch (HttpError error)
      {
        return StatusCode(error.Code, error);
      }
    }

    [HttpGet("verification/{id:length(24)}")]
    public async Task<ActionResult> Verification(string id)
    {
      try
      {
        await _authService.Verification(id);

        return StatusCode(200);
      }
      catch (HttpError error)
      {
        return StatusCode(error.Code, error);
      }
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<AuthResponse>> Login(AuthRequest request)
    {
      try
      {
        return await _authService.Login(request);
      }
      catch (HttpError error)
      {
        return StatusCode(error.Code, error);
      }
    }

    [HttpGet("authenticate")]
    [ServiceFilter(typeof(ClientAuthorize))]
    public ActionResult<User> Authenticate()
    {
      return _jwtHolder.User;
    }

    [HttpPost("step-two")]
    [ServiceFilter(typeof(ClientAuthorize))]
    public async Task<ActionResult> StepTwoUpdate(StepTwoUpdateRequest request) {
      var user = _jwtHolder.User;

      await _userService.StepTwoUpdate(user, request);

      return StatusCode(201);
    }
  }
}