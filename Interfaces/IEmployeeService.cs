using System;
using System.Linq.Expressions;
using _99phantram.Entities;

namespace _99phantram.Interfaces
{
  public interface IEmployeeService
  {
    Employee FindOne(Expression<Func<Employee, bool>> filter);
  }
}