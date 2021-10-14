using _99phantram.Entities;
using FluentValidation;

namespace _99phantram.Models
{
  public class PostUserBody
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Gender Sex { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string Avatar { get; set; }
    public string Role { get; set; }
    public UserStatus Status { get; set; }
  }

  public class PostUserBodyValidator : AbstractValidator<PostUserBody>
  {
    public PostUserBodyValidator()
    {
      RuleFor(r => r.FirstName).NotEmpty();
      RuleFor(r => r.LastName).NotEmpty();
      RuleFor(r => r.Email).NotEmpty().EmailAddress();
      RuleFor(r => r.Password).NotEmpty().MinimumLength(7);
      RuleFor(r => r.Sex).IsInEnum();
      RuleFor(r => r.Address).NotEmpty();
      RuleFor(r => r.PhoneNumber).NotEmpty().Length(10);
      RuleFor(r => r.Status).IsInEnum();
    }
  }
}