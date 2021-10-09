using System;
using System.Linq.Expressions;
using _99phantram.Entities;
using _99phantram.Interfaces;
using MongoDB.Driver;

namespace _99phantram.Services
{
  public class EmployeeService : IEmployeeService
  {
    private IMongoCollection<Employee> _collection;
    private string _hashed;
    private IAuthService _authSerivce;
    private IRoleService _roleService;

    public EmployeeService(IDatabaseContext databaseContext, IAuthService authService, IRoleService roleService)
    {
      _authSerivce = authService;
      _roleService = roleService;
      _collection = databaseContext.Database.GetCollection<Employee>("employees");
      _hashed = authService.EncryptPassword("password");
      
      _collection.Indexes.CreateOne(new CreateIndexModel<Employee>(Builders<Employee>.IndexKeys.Ascending(e => e.Username), new CreateIndexOptions<Employee> { Unique = true }));
    }

    public Employee FindOne(Expression<Func<Employee, bool>> expression)
    {
      var filter = Builders<Employee>.Filter.Where(expression);
      return _collection.Find(filter).FirstOrDefault();
    }
  }
}