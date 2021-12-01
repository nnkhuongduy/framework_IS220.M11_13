using System;
using _99phantram.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

namespace _99phantram.Entities
{
  public enum RatingStatus
  {
    ACTIVE,
    HIDDEN,
    ARCHIVE,
  }

  [Collection("ratings")]
  public class Rating : Entity, ICreatedOn, IModifiedOn
  {
    [BsonIgnore]
    public UserSnapshot User { get; set; }
    [Field("user")]
    public One<User> UserRef { get; set; }
    [BsonIgnore]
    public UserSnapshot RatingOn { get; set; }
    [Field("rating_on")]
    public One<User> RatingOnRef { get; set; }
    [BsonIgnore]
    public SupplySnapshot Supply { get; set; }
    [Field("supply")]
    public One<Supply> SupplyRef { get; set; }
    [BsonIgnore]
    public Order Order { get; set; }
    [Field("order")]
    public One<Order> OrderRef { get; set; }
    [Field("point")]
    public int Point { get; set; }
    [Field("comment")]
    public string Comment { get; set; }
    [Field("status")]
    public RatingStatus Status { get; set; }
    [Field("created_on")]
    public DateTime CreatedOn { get; set; }
    [Field("modified_on")]
    public DateTime ModifiedOn { get; set; }
  }
}