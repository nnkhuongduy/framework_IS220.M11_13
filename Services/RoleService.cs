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

    public Role FindOne(Expression<Func<Role, bool>> expression)
    {
      var filter = Builders<Role>.Filter.Where(expression);

      return _collection.Find(filter).FirstOrDefault();
    }
  }
}