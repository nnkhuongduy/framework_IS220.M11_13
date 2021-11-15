using System;
using MongoDB.Bson;
using MongoDB.Entities;

namespace _99phantram.Entities
{
  [Collection("vendors")]
  public class Vendor : Entity, ICreatedOn, IModifiedOn
  {
    [Field("user")]
    public ObjectId User { get; set; }
    [Field("services")]
    public ObjectId Services { get; set; }
    [Field("description")]
    public string Description { get; set; }
    [Field("images")]
    public String[] Images { get; set; }
    [Field("created_on")]
    public DateTime CreatedOn { get; set; }
    [Field("modified_on")]
    public DateTime ModifiedOn { get; set; }
  }
}
