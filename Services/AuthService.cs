using BC = BCrypt.Net.BCrypt;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using _99phantram.Entities;
using System.Security.Claims;
using _99phantram.Models;
using _99phantram.Interfaces;

namespace _99phantram.Services
{
  public class AuthService : IAuthService
  {
    private IConfiguration _config;

    public AuthService(IConfiguration config)
    {
      _config = config;
    }

    public string EncryptPassword(string password)
    {
      return BC.HashPassword(password, BC.GenerateSalt());
    }

    public bool VerifyPassword(string password, string hash)
    {
      return BC.Verify(password, hash);
    }

    public AuthResponse Authenticate(User indentifier, bool isRemember = false)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(_config["JWT:SecretKey"]);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new[] { new Claim("id", indentifier.Id) }),
        Expires = DateTime.UtcNow.AddHours(isRemember ? 24 : 2),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);
      var tokenString = tokenHandler.WriteToken(token);

      return new AuthResponse(indentifier, tokenString);
    }

    public JwtSecurityToken VerifyToken(string token)
    {
      try
      {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_config["JWT:SecretKey"]);

        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(key),
          ValidateIssuer = false,
          ValidateAudience = false,
          ClockSkew = TimeSpan.Zero,
        }, out SecurityToken validatedToken);

        return (JwtSecurityToken)validatedToken;
      }
      catch (SecurityTokenExpiredException exception)
      {
        throw exception;
      }
    }
  }
}