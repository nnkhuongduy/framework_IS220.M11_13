using System.IdentityModel.Tokens.Jwt;
using _99phantram.Entities;
using _99phantram.Models;

namespace _99phantram.Interfaces
{
  public interface IAuthService
  {
    string EncryptPassword(string password);

    bool VerifyPassword(string hash, string password);

    AuthResponse Authenticate(UserBase employee, bool isRemember = false);

    JwtSecurityToken VerifyToken(string token);
  }
}