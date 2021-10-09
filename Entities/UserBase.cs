using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace _99phantram.Entities
{
  public class UserBase
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    [BsonElement("username")]
    [BsonRequired]
    public string Username { get; set; }
    [BsonElement("password")]
    [BsonRequired]
    public string Password { get; set; }
    [BsonElement("first_name")]
    [BsonRequired]
    public string FirstName { get; set; }
    [BsonElement("last_name")]
    [BsonRequired]
    public string LastName { get; set; }
  }
}