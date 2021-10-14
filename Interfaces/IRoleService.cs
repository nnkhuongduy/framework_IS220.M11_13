using System;
using System.Linq.Expressions;
using _99phantram.Entities;
using MongoDB.Driver;

namespace _99phantram.Interfaces
{
  public interface IRoleService
  {
    IFindFluent<Role, Role> GetRole(Expression<Func<Role, bool>> expression);
    IFindFluent<Role, Role> GetRole(FilterDefinition<Role> query);
    IFindFluent<Role, Role> GetRoles(Expression<Func<Role, bool>> expression);
    IFindFluent<Role, Role> GetRoles(FilterDefinition<Role> query);
  }
}