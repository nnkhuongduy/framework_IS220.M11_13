using BC = BCrypt.Net.BCrypt;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;
using MongoDB.Entities;

using _99phantram.Entities;
using _99phantram.Models;
using _99phantram.Interfaces;

namespace _99phantram.Services
{
  public class AuthService : IAuthService
  {
    private IConfiguration _config;
    private readonly IMailService _mailService;

    public AuthService(IConfiguration config, IMailService mailService)
    {
      _config = config;
      _mailService = mailService;
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
        Subject = new ClaimsIdentity(new[] { new Claim("id", indentifier.ID) }),
        Expires = DateTime.UtcNow.AddHours(isRemember ? 24 : 2),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);
      var tokenString = tokenHandler.WriteToken(token);

      return new AuthResponse(indentifier, tokenString);
    }

    public JwtSecurityToken VerifyToken(string token)
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

    public async Task<User> Register(UserRegistrationBody body)
    {
      var role = await DB.Find<Role>().Match(_ => _.Name == "Buyer").ExecuteFirstAsync();

      var existedUser = await DB.Find<User>().Match(_ => _.Email == body.Email).ExecuteFirstAsync();

      if (existedUser != null) {
        throw new HttpError(false, 400, "Tài khoản này đã tồn tại!");
      }

      Entities.User user = new Entities.User();

      user.Email = body.Email;
      user.Password = EncryptPassword(body.Password);
      user.FirstName = body.FirstName;
      user.LastName = body.LastName;
      user.Sex = Gender.Other;
      user.Address = "";
      user.PhoneNumber = "";
      user.Role = role;
      user.Status = UserStatus.CREATED;
      user.Oauth = false;
      user.OauthProvider = OAuthProvider.None;
      user.Avatar = "";

      await user.SaveAsync();

      _mailService.SendRegistrationVerification(user);

      return user;
    }

    public async Task Verification(string id) {
      var user = await DB.Find<User>().MatchID(id).ExecuteFirstAsync();

      if (user == null) {
        throw new HttpError(false, 404, "Không tìm thấy tài khoản này!");
      }

      if (user.Status != UserStatus.CREATED) {
        throw new HttpError(false, 400, "Tài khoản này đã được kích hoạt!");
      }

      user.Status = UserStatus.VERIFIED;

      await user.SaveAsync();
    }

    public async Task<AuthResponse> Login(AuthRequest request)
    {
      var user = await DB.Find<User>().Match(e => e.Email == request.Email).ExecuteFirstAsync();

      if (user == null)
      {
        throw new HttpError(false, 404, "Không tìm thấy người dùng!");
      }

      if (user.Status == UserStatus.CREATED)
      {
        throw new HttpError(false, 400, "Tài khoản chưa được xác thực! Vui lòng xác thực qua email.");
      }

      if (user.Status == UserStatus.ARCHIVED)
      {
        throw new HttpError(false, 400, "Tài khoản đã bị khóa hoặc xóa! Vui lòng liên hệ 99phantram.");
      }

      if (user.Role.RoleLevel == RoleLevel.APP)
      {
        throw new HttpError(false, 400, "Tài khoản không có quyền truy cập tài nguyên này!");
      }

      if (!VerifyPassword(request.Password, user.Password))
      {
        throw new HttpError(false, 400, "Mật khẩu không đúng!");
      }

      var authResponse = Authenticate(user, request.Remember == "true");

      return authResponse;
    }

    public async Task<AuthResponse> LoginEmployee(AuthRequest request)
    {
      var user = await DB.Find<User>().Match(e => e.Email == request.Email).ExecuteFirstAsync();

      if (user == null)
      {
        throw new HttpError(false, 404, "Không tìm thấy người dùng!");
      }

      if (user.Status == UserStatus.CREATED)
      {
        throw new HttpError(false, 400, "Tài khoản chưa được xác thực! Vui lòng xác thực qua email.");
      }

      if (user.Status == UserStatus.ARCHIVED)
      {
        throw new HttpError(false, 400, "Tài khoản đã bị khóa hoặc xóa!");
      }

      if (user.Role.RoleLevel == RoleLevel.CLIENT)
      {
        throw new HttpError(false, 400, "Tài khoản không có quyền truy cập tài nguyên này!");
      }

      if (!VerifyPassword(request.Password, user.Password))
      {
        throw new HttpError(false, 400, "Mật khẩu không đúng!");
      }

      var authResponse = Authenticate(user, request.Remember == "true");

      return authResponse;
    }
  }
}