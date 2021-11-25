using System.Collections.Generic;
using FluentValidation;

using _99phantram.Entities;

namespace _99phantram.Models
{
  public class ServicePostBody
  {
    public string ServiceType { get; set; }
    public string Name { get; set; }
    public Dictionary<string, object> Value { get; set; }
    public ServiceStatus Status { get; set; }
  }

  public class ServicePostBodyValidator : AbstractValidator<ServicePostBody>
  {
    public ServicePostBodyValidator()
    {
      RuleFor(r => r.Name).NotEmpty();
      RuleFor(r => r.ServiceType).NotEmpty();
      RuleFor(r => r.Value).NotEmpty();
      RuleFor(r => r.Status).IsInEnum();
    }
  }

  public class ServicePutBody
  {
    public string Name { get; set; }
    public Dictionary<string, object> Value { get; set; }
    public ServiceStatus Status { get; set; }
  }

  public class ServicePutBodyValidator : AbstractValidator<ServicePutBody>
  {
    public ServicePutBodyValidator()
    {
      RuleFor(r => r.Name).NotEmpty();
      RuleFor(r => r.Value).NotEmpty();
      RuleFor(r => r.Status).IsInEnum();
    }
  }
}
