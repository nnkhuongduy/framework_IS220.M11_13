using System;
using System.Linq.Expressions;
using _99phantram.Entities;
using MongoDB.Driver;

namespace _99phantram.Interfaces
{
  public interface IUserService
  {
    IFindFluent<User, User> GetUser(Expression<Func<User, bool>> expression);
    IFindFluent<User, User> GetUser(FilterDefinition<User> query);
    IFindFluent<User, User> GetUsers(Expression<Func<User, bool>> expression);
    IFindFluent<User, User> GetUsers(FilterDefinition<User> query);
    void CreateUser(User user);
  }
}