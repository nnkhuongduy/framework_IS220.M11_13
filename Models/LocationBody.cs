using System;
using System.Collections.Generic;
using _99phantram.Entities;
using FluentValidation;

namespace _99phantram.Models
{
public class LocationBody
{
    public string Name{ get; set;}
    public LocationLevel LocationLevel{ get; set; }
    public LocationStatus Status { get; set; }
    public List<string> SubLocations{ get; set;}
}
public class LocationBodyValidator: AbstractValidator<LocationBody>
{
    public LocationBodyValidator()
    {
      RuleFor(r => r.Name).NotEmpty();
      RuleFor(r => r.LocationLevel).IsInEnum();
      RuleFor(r => r.Status).IsInEnum();
    }
}
}