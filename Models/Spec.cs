using FluentValidation;

namespace _99phantram.Models
{
  public class SpecBody
  {
    public string Name { get; set; }
    public bool Required { get; set; }
  }

  public class SpecBodyValidator : AbstractValidator<SpecBody>
  {
    public SpecBodyValidator()
    {
      RuleFor(r => r.Name).NotEmpty();
    }
  }
}