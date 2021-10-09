using System.Linq;
using _99phantram.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace _99phantram.Helpers
{
  public class EmployeeAuthorize : IAuthorizationFilter
  {
    private IEmployeeService _employeeService;
    private IAuthService _authService;

    public EmployeeAuthorize(IEmployeeService employeeService, IAuthService authService)
    {
      _employeeService = employeeService;
      _authService = authService;
    }
    public void OnAuthorization(AuthorizationFilterContext context)
    {
      try
      {
        var token = (string)context.HttpContext.Items["JwtToken"];
        var verifiedToken = _authService.VerifyToken(token);
        var empId = verifiedToken.Claims.First(x => x.Type == "id").Value;

        var employee = _employeeService.FindOne(e => e.Id == empId);

        if (employee == null)
        {
          context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
        else
        {
          context.HttpContext.Items["Employee"] = employee;
        }
      }
      catch (SecurityTokenExpiredException)
      {
        context.Result = new JsonResult(new { message = "Token has expired!" }) { StatusCode = StatusCodes.Status401Unauthorized };
      }
    }
  }
}
