using System.Linq;
using System.Threading.Tasks;
using _99phantram.Entities;
using _99phantram.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using MongoDB.Entities;

namespace _99phantram.Helpers
{
  public class AppAuthorize : IAuthorizationFilter
  {
    private IAuthService _authService;

    public AppAuthorize(IAuthService authService)
    {
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
          var user = Run.Sync(() =>
            DB.Find<User>()
              .Match(user => 
                user.ID.Equals(empId) &&
                user.Role.RoleLevel != Entities.RoleLevel.CLIENT &&
                user.Status == Entities.UserStatus.VERIFIED)
              .ExecuteFirstAsync()
          );

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
