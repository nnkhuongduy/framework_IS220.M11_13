using System.Collections.Generic;
using MongoDB.Bson;
using FluentValidation;

using _99phantram.Entities;

namespace _99phantram.Models
{
  public class LocationBody
  {
    public string Name { get; set; }
    public LocationLevel LocationLevel { get; set; }
    public LocationStatus Status { get; set; }
    public List<string> SubLocations { get; set; }
  }

  public class LocationBodyValidator : AbstractValidator<LocationBody>
  {
    public LocationBodyValidator()
    {
      RuleFor(_ => _.Name).NotEmpty();
      RuleFor(_ => _.LocationLevel).IsInEnum();
      RuleFor(_ => _.Status).IsInEnum();
    }
  }

  public class LocationSnapshot
  {
    public string Name { get; set; }
  }
}