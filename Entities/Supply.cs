using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

namespace _99phantram.Entities
{
  public enum SupplyStatus
  {
    WAITING,
    DECLINED,
    ACTIVE,
    SOLD,
    ARCHIVED
  }

  [Collection("supplies")]
  public class Supply : Entity, ICreatedOn, IModifiedOn
  {
    [BsonIgnore]
    public User Owner { get; set; }
    [Field("owner")]
    public One<User> OwnerRef { get; set; }
    [Field("name")]
    public string Name { get; set; }
    [Field("price")]
    public long Price { get; set; }
    [Field("description")]
    public string Description { get; set; }
    [Field("services")]
    public List<Service> Services { get; set; }
    [Field("specs")]
    public List<Spec> Specs { get; set; }
    [Field("images")]
    public List<string> Images { get; set; }
    [Field("thumbnail")]
    public string Thumbnail { get; set; }
    [Field("categories")]
    public List<Category> Categories { get; set; }
    [Field("locations")]
    public List<Location> Locations { get; set; }
    [Field("address")]
    public string Address { get; set; }
    [Field("reason")]
    public string Reason { get; set; }
    [Field("product_status")]
    public ProductStatus ProductStatus { get; set; }
    [Field("status")]
    public SupplyStatus Status { get; set; }
    [Field("created_on")]
    public DateTime CreatedOn { get; set; }
    [Field("modified_on")]
    public DateTime ModifiedOn { get; set; }
  }
}
