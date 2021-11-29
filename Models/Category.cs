using _99phantram.Entities;
using FluentValidation;

namespace _99phantram.Models
{
  public class PostCategoryBody
  {
    public string Name { get; set; }
    public string Image { get; set; }
    public CategoryLevel CategoryLevel { get; set; }
    public CategoryStatus Status { get; set; }
  }

  public class PostCategoryBodyValidator : AbstractValidator<PostCategoryBody>
  {
    public PostCategoryBodyValidator()
    {
      RuleFor(r => r.Name).NotEmpty();
      RuleFor(r => r.CategoryLevel).IsInEnum();
      RuleFor(r => r.Status).IsInEnum();
    }
  }

  public class PutCategoryBody
  {
    public string Name { get; set; }
    public string Image { get; set; }
    public CategoryLevel CategoryLevel { get; set; }
    public CategoryStatus Status { get; set; }
    public string[] SubCategories { get; set; }
  }

  public class PutCategoryBodyValidator : AbstractValidator<PutCategoryBody>
  {
    public PutCategoryBodyValidator()
    {
      RuleFor(r => r.Name).NotEmpty();
      RuleFor(r => r.CategoryLevel).IsInEnum();
      RuleFor(r => r.Status).IsInEnum();
    }
  }
}