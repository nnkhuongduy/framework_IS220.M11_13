using System;
using MongoDB.Bson;
using MongoDB.Entities;

namespace _99phantram.Entities
{
  public enum DemandStatus
  {
    CREATED,
    ACTIVE,
    EXPIRED,
  }

  public enum ProductStatus
  {
    _99,
    _90,
    _80,
    _50
  }

  [Collection("demands")]
  public class Demand : Entity, ICreatedOn, IModifiedOn
  {
    [Field("owner")]
    public ObjectId Owner { get; set; }
    [Field("name")]
    public string Name { get; set; }
    [Field("price")]
    public long Price { get; set; }
    [Field("description")]
    public string Description { get; set; }
    [Field("service")]
    public Service[] Services { get; set; }
    [Field("specs")]
    public Spec[] Specs { get; set; }
    [Field("categories")]
    public Category[] Categories { get; set; }
    [Field("locations")]
    public Location[] Locations { get; set; }
    [Field("product_status")]
    public ProductStatus ProductStatus { get; set; }
    [Field("status")]
    public DemandStatus Status { get; set; }
    [Field("created_on")]
    public DateTime CreatedOn { get; set; }
    [Field("modified_on")]
    public DateTime ModifiedOn { get; set; }
  }
}
