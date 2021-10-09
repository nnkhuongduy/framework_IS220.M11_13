using MongoDB.Bson.Serialization.Attributes;

namespace _99phantram.Entities
{
  public class Employee : UserBase
  {
    [BsonRequired]
    [BsonElement("role")]
    public Role Role { get; set; }
  }
}