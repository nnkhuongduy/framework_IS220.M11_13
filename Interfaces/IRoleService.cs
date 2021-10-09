using System;
using System.Linq.Expressions;
using _99phantram.Entities;

namespace _99phantram.Interfaces
{
  public interface IRoleService
  {
    Role FindOne(Expression<Func<Role, bool>> expression);
  }
}