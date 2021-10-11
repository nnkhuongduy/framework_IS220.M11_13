using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace _99phantram.Entities
{
  public enum RoleLevel
  {
    CLIENT = 1,
    APP = 2,
    ALL = 3
  }
  public class Role
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    [BsonRequired]
    [BsonElement("name")]
    public string Name { get; set; }
    [BsonElement("role_level")]
    [BsonRequired]
    public RoleLevel RoleLevel { get; set; }
    [BsonElement("selectable_roles")]
    public ObjectId[] SelectableRoles { get; set; }
  }
}