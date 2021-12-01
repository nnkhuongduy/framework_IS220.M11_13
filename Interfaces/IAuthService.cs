using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

using _99phantram.Entities;
using _99phantram.Models;

namespace _99phantram.Interfaces
{
  public interface IAuthService
  {
    string EncryptPassword(string password);

    bool VerifyPassword(string hash, string password);

    AuthResponse Authenticate(User user, bool isRemember = false);

    JwtSecurityToken VerifyToken(string token);

    Task<User> Register(UserRegistrationBody body);

    Task Verification(string id);

    Task<AuthResponse> Login(AuthRequest request);
    Task<AuthResponse> LoginEmployee(AuthRequest request);
  }
}