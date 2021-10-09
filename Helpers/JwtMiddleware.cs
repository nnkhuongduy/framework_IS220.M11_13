using System.Linq;
using System.Threading.Tasks;
using _99phantram.Interfaces;
using Microsoft.AspNetCore.Http;

namespace _99phantram.Helpers
{
  public class JwtMiddleWare
  {
    private readonly RequestDelegate _next;
    private readonly IAuthService _authService;

    public JwtMiddleWare(RequestDelegate next, IAuthService authService)
    {
      _next = next;
      _authService = authService;
    }

    public async Task Invoke(HttpContext context)
    {
      var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

      if (token != null)
      {
        context.Items["JwtToken"] = token;
      }

      await _next(context);
    }
  }
}