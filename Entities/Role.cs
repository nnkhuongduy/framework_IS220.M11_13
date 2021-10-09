using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace _99phantram.Entities
{
  public class Role
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    [BsonRequired]
    [BsonElement("name")]
    public string Name { get; set; }
  }
}