using System.Threading.Tasks;
using MongoDB.Entities;
using System.Collections.Generic;

using _99phantram.Entities;
using _99phantram.Interfaces;
using _99phantram.Models;

namespace _99phantram.Services
{
  public class UserService : IUserService
  {
    private readonly IRoleService _roleService;
    private readonly IAuthService _authService;

    public UserService(IAuthService authService, IRoleService roleService)
    {
      _roleService = roleService;
      _authService = authService;

      Task.Run(async () =>
      {
        await DB.Index<User>()
          .Key(user => user.Email, KeyType.Ascending)
          .Option(option => option.Unique = true)
          .CreateAsync();
      }).GetAwaiter().GetResult();
    }

    public async Task<List<User>> GetAllUsers(bool withPassword)
    {
      var query = DB.Find<User>().Match(_ => true).Sort(_ => _.CreatedOn, MongoDB.Entities.Order.Descending);

      if (!withPassword)
      {
        query.Project(_ => _.Exclude("password"));
      }

      return await query.ExecuteAsync();
    }

    public async Task<User> GetUser(string id, bool withPassword)
    {
      var query = DB.Find<User>().MatchID(id);

      if (!withPassword)
      {
        query.Project(_ => _.Exclude("password"));
      }

      var result = await query.ExecuteFirstAsync();

      if (result == null)
      {
        throw new HttpError(false, 404, "Không tìm thấy người dùng");
      }

      return result;
    }

    public async Task<User> CreateUser(PostUserBody body)
    {
      var role = await _roleService.GetRole(body.Role);

      Entities.User user = new Entities.User();

      user.Email = body.Email;
      user.Password = _authService.EncryptPassword(body.Password);
      user.FirstName = body.FirstName;
      user.LastName = body.LastName;
      user.Sex = body.Sex;
      user.Address = body.Address;
      user.PhoneNumber = body.PhoneNumber;
      user.Role = role;
      user.Status = body.Status;
      user.Oauth = false;
      user.OauthProvider = OAuthProvider.None;
      user.Avatar = body.Avatar;

      await user.SaveAsync();

      return user;
    }

    public async Task<User> UpdateUser(string id, PutUserBody body)
    {
      var user = await GetUser(id, true);

      var role = await _roleService.GetRole(body.Role);

      var newUser = await DB.UpdateAndGet<User>()
        .MatchID(id)
        .Modify(user => user.Email, body.Email)
        .Modify(user => user.Password, string.IsNullOrEmpty(body.Password) ? user.Password : _authService.EncryptPassword(body.Password))
        .Modify(user => user.FirstName, body.FirstName)
        .Modify(user => user.LastName, body.LastName)
        .Modify(user => user.Sex, body.Sex)
        .Modify(user => user.Address, body.Address)
        .Modify(user => user.PhoneNumber, body.PhoneNumber)
        .Modify(user => user.Role, role)
        .Modify(user => user.Status, body.Status)
        .Modify(user => user.Avatar, body.Avatar)
        .Project(user => user.Exclude("password"))
        .ExecuteAsync();

      return newUser;
    }

    public async Task<User> ArchiveUser(User user)
    {
      user.Status = UserStatus.ARCHIVED;

      await user.SaveAsync();

      return user;
    }

    public async Task DeleteUser(string id)
    {
      var user = await GetUser(id, false);

      await user.DeleteAsync();

      return;
    }
  }
}