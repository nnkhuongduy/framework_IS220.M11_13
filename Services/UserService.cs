using System.Threading.Tasks;
using _99phantram.Entities;
using _99phantram.Interfaces;
using MongoDB.Entities;

namespace _99phantram.Services
{
  public class UserService : IUserService
  {
    public UserService(IAuthService authService)
    {
      Task.Run(async () =>
      {
        await DB.Index<User>()
          .Key(user => user.Email, KeyType.Ascending)
          .Option(option => option.Unique = true)
          .CreateAsync();
      }).GetAwaiter().GetResult();
    }
  }
}