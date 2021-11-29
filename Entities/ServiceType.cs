using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace _99phantram.Entities
{
  public enum ServiceTypeStatus
  {
    ACTIVE,
    DEACTIVE
  }

  [Collection("service_types")]
  public class ServiceType : Entity
  {
    [Field("name")]
    public string Name { get; set; }
    [Field("status")]
    public ServiceTypeStatus Status { get; set; }
    [BsonIgnore]
    public Dictionary<string, object> Value { get; set; }
    [JsonIgnore]
    [Field("value")]
    public BsonDocument ValueBson {get; set;}
  }
}