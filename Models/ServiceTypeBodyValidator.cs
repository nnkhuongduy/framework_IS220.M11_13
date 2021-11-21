using FluentValidation;
namespace _99phantram.Models
{
  public class ServiceTypeBodyValidator : AbstractValidator<ServiceTypeBody>
  {
    public ServiceTypeBodyValidator()
    {
      RuleFor(r => r.Name).NotEmpty();
      RuleFor(r => r.Value).NotEmpty();
      RuleFor(r => r.Status).IsInEnum();
    }
  }
}