using System.Collections.Generic;
using FluentValidation;

using _99phantram.Entities;

namespace _99phantram.Models
{
  public class ClientPostSupplySpec
  {
    public string Id { get; set; }
    public string Value { get; set; }
  }

  public class ClientPostSupply
  {
    public string Name { get; set; }
    public long Price { get; set; }
    public string Description { get; set; }
    public List<ClientPostSupplySpec> Specs { get; set; }
    public List<string> Images { get; set; }
    public string Thumbnail { get; set; }
    public List<string> Categories { get; set; }
    public List<string> Locations { get; set; }
    public string Address { get; set; }
  }

  public class ClientPostSupplySpecValidator : AbstractValidator<ClientPostSupplySpec>
  {
    public ClientPostSupplySpecValidator()
    {
      RuleFor(_ => _.Id).NotEmpty();
      RuleFor(_ => _.Value).NotEmpty();
    }
  }

  public class ClientPostSupplyValidator : AbstractValidator<ClientPostSupply>
  {
    public ClientPostSupplyValidator()
    {
      RuleFor(_ => _.Name).NotEmpty();
      RuleFor(_ => _.Price).NotEmpty().GreaterThan(0);
      RuleFor(_ => _.Description).NotEmpty().Length(0, 500);
      RuleFor(_ => _.Specs).NotEmpty().Must(_ => _.Count != 0);
      RuleFor(_ => _.Images).NotEmpty().Must(_ => _.Count != 0);
      RuleFor(_ => _.Thumbnail).NotEmpty();
      RuleFor(_ => _.Categories).NotEmpty().Must(_ => _.Count == 2);
      RuleFor(_ => _.Locations).NotEmpty().Must(_ => _.Count == 3);
      RuleFor(_ => _.Address).NotEmpty();
    }
  }

  public class SupplyQueryFilter
  {
    public int Page { get; set; }
  }

  public class PutSupply
  {
    public SupplyStatus Status { get; set; }
    public bool SendEmail { get; set; }
    public string Reason { get; set; }
  }

  public class PutSupplyValidator : AbstractValidator<PutSupply>
  {
    public PutSupplyValidator()
    {
      RuleFor(_ => _.Status).IsInEnum();
      RuleFor(_ => _.SendEmail).NotEmpty();
    }
  }

  public class SupplySnapshot
  {
    public string ID { get; set; }
    public string Name { get; set; }
    public long Price { get; set; }
    public string Thumbnail { get; set; }
    public List<LocationSnapshot> Locations { get; set; }
  }
}
