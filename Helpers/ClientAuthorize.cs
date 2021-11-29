using System.Linq;
using System.Threading.Tasks;
using _99phantram.Entities;
using _99phantram.Interfaces;
using _99phantram.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using MongoDB.Entities;

namespace _99phantram.Helpers
{
  public class ClientAuthorize : IAuthorizationFilter
  {
    private JwtHolder _jwtHolder;
    private IAuthService _authService;

    public ClientAuthorize(IAuthService authService, JwtHolder jwtHolder)
    {
      _jwtHolder = jwtHolder;
      _authService = authService;
    }
    public void OnAuthorization(AuthorizationFilterContext context)
    {
      string token = _jwtHolder.Token;

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
              .Project(user => user.Exclude("password"))
              .ExecuteFirstAsync()
          );

          if (user == null)
            context.Result = new JsonResult(new HttpError(false, 404, "Không tìm thấy tài khoản")) { StatusCode = StatusCodes.Status404NotFound };

          if (user.Status != UserStatus.VERIFIED)
            context.Result = new JsonResult(new HttpError(false, 400, "Tài khoản chưa được xác thực")) { StatusCode = StatusCodes.Status400BadRequest };

          if (
            (string.IsNullOrEmpty(user.PhoneNumber) ||
            string.IsNullOrEmpty(user.Address) ||
            string.IsNullOrEmpty(user.LocationBlockRef.ID) ||
            string.IsNullOrEmpty(user.LocationProvinceRef.ID) ||
            string.IsNullOrEmpty(user.LocationWardRef.ID)) &&
            context.HttpContext.Request.Path.Value != "/api/auth/step-two"
          )
            context.Result = new JsonResult(new HttpError(false, 403, "Tài khoản cần cập nhật thông tin")) { StatusCode = StatusCodes.Status403Forbidden };

          _jwtHolder.User = user;
        }
        catch (SecurityTokenExpiredException)
        {
          context.Result = new JsonResult(new HttpError(false, 401, "Token đã hết hạn")) { StatusCode = StatusCodes.Status401Unauthorized };
        }
      }
      else
        context.Result = new JsonResult(new HttpError(false, 401, "Unauthorized")) { StatusCode = StatusCodes.Status401Unauthorized };
    }
  }
}
