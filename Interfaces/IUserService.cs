using System.Collections.Generic;
using System.Threading.Tasks;

using _99phantram.Entities;
using _99phantram.Models;

namespace _99phantram.Interfaces
{
  public interface IUserService
  {
    Task<List<User>> GetAllUsers(bool withPassword);
    Task<User> GetUser(string id, bool withPassword);
    Task<User> CreateUser(PostUserBody body);
    Task<User> UpdateUser(string id, PutUserBody body);
    Task<User> ArchiveUser(User user);
    Task DeleteUser(string id);
  }
}