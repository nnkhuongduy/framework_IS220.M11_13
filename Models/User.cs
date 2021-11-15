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

  public class PutUserBody
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

  public class PutUserBodyValidator : AbstractValidator<PutUserBody>
  {
    public PutUserBodyValidator()
    {
      RuleFor(r => r.FirstName).NotEmpty();
      RuleFor(r => r.LastName).NotEmpty();
      RuleFor(r => r.Email).NotEmpty().EmailAddress();
      RuleFor(r => r.Password).MinimumLength(7).When(r => !string.IsNullOrEmpty(r.Password));
      RuleFor(r => r.Sex).IsInEnum();
      RuleFor(r => r.Address).NotEmpty();
      RuleFor(r => r.PhoneNumber).NotEmpty().Length(10);
      RuleFor(r => r.Status).IsInEnum();
    }
  }

  public class UserResponse {
    public MongoDB.Bson.ObjectId id { get; set; }
    public string email { get; set; }
    public string first_name { get; set; }
    public string last_name { get; set; }
    public Gender sex { get; set; }
    public string address { get; set; }
    public string phone_number { get; set; }
    public string avatar { get; set; }
    public bool oauth { get; set; }
    public OAuthProvider oauth_provider { get; set; }
    public Role role { get; set; }
    public UserStatus status { get; set; }
    public System.DateTime created_at { get; set; }
  }
}