using System;
using System.Linq.Expressions;
using _99phantram.Entities;
using _99phantram.Interfaces;
using MongoDB.Driver;

namespace _99phantram.Services
{
  public class RoleService : IRoleService
  {
    private IMongoCollection<Role> _collection;

    public RoleService(IDatabaseContext databaseContext)
    {
      _collection = databaseContext.Database.GetCollection<Role>("roles");
    }

    public IFindFluent<Role, Role> GetRole(Expression<Func<Role, bool>> expression)
    {
      var query = Builders<Role>.Filter.Where(expression);
      return _collection.Find(query);
    }

    public IFindFluent<Role, Role> GetRole(FilterDefinition<Role> query)
    {
      return _collection.Find(query);
    }

    public IFindFluent<Role, Role> GetRoles(Expression<Func<Role, bool>> expression)
    {
      var query = Builders<Role>.Filter.Where(expression);
      return _collection.Find(query);
    }

    public IFindFluent<Role, Role> GetRoles(FilterDefinition<Role> query)
    {
      return _collection.Find(query);
    }
  }
}