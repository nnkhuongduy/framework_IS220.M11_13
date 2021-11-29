using System.Collections.Generic;
using FluentValidation;

using _99phantram.Entities;

namespace _99phantram.Models
{
  public class ServiceTypeBody
  {
    public string Name { get; set; }
    public Dictionary<string, object> Value { get; set; }
    public ServiceTypeStatus Status { get; set; }
  }

  public class ServiceTypeBodyValidator : AbstractValidator<ServiceTypeBody>
  {
    public ServiceTypeBodyValidator()
    {
      RuleFor(_ => _.Name).NotEmpty();
      RuleFor(_ => _.Value).NotEmpty();
      RuleFor(_ => _.Status).IsInEnum();
    }
  }
}