using MongoDB.Entities;

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
  }
}