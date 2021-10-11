using System;
using System.Linq.Expressions;
using _99phantram.Entities;
using _99phantram.Interfaces;
using MongoDB.Driver;

namespace _99phantram.Services
{
  public class UserService : IUserService
  {
    private IMongoCollection<User> _collection;
    private IAuthService _authSerivce;
    private IRoleService _roleService;

    public UserService(IDatabaseContext databaseContext, IAuthService authService, IRoleService roleService)
    {
      _authSerivce = authService;
      _roleService = roleService;
      _collection = databaseContext.Database.GetCollection<User>("users");

      _collection.Indexes.CreateOne(new CreateIndexModel<User>(Builders<User>.IndexKeys.Ascending(e => e.Email), new CreateIndexOptions<User> { Unique = true }));
    }

    public IFindFluent<User, User> GetUser(Expression<Func<User, bool>> expression)
    {
      var query = Builders<User>.Filter.Where(expression);
      return _collection.Find(query);
    }
    
    public IFindFluent<User, User> GetUser(FilterDefinition<User> query)
    {
      return _collection.Find(query);
    }

    public IFindFluent<User, User> GetUsers(Expression<Func<User, bool>> expression)
    {
      var query = Builders<User>.Filter.Where(expression);
      return _collection.Find(query);
    }

    public IFindFluent<User, User> GetUsers(FilterDefinition<User> query)
    {
      return _collection.Find(query);
    }
  }
}