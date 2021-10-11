using System.Linq;
using _99phantram.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace _99phantram.Helpers
{
  public class AppAuthorize : IAuthorizationFilter
  {
    private IUserService _userService;
    private IAuthService _authService;

    public AppAuthorize(IUserService userService, IAuthService authService)
    {
      _userService = userService;
      _authService = authService;
    }
    public void OnAuthorization(AuthorizationFilterContext context)
    {
      string token = (string)context.HttpContext.Items["JwtToken"];

      if (token != null)
      {
        try
        {
          var verifiedToken = _authService.VerifyToken(token);
          var empId = verifiedToken.Claims.First(x => x.Type == "id").Value;
          var user = _userService.GetUser(e => e.Id == empId && e.Role.RoleLevel != Entities.RoleLevel.CLIENT).FirstOrDefault();

          if (user != null)
            context.HttpContext.Items["User"] = user;
          else
            context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
        catch (SecurityTokenExpiredException)
        {
          context.Result = new JsonResult(new { message = "Token has expired!" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
      }
      else
        context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
    }
  }
}
