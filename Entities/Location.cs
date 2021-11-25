using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;
using MongoDB.Bson;
using System.Collections.Generic;

namespace _99phantram.Entities
{
  public enum LocationLevel
  {
    PROVINCE,
    WARD,
    BLOCK
  }
  public enum LocationStatus
  {
    NEW,
    ACTIVE,
    ARCHIVED
  }
  [Collection("locations")]
  public class Location : Entity
  {
    [Field("name")]
    public string Name { get; set; }
    [Field("location_level")]
    public LocationLevel LocationLevel { get; set; }
    [Field("status")]
    [BsonDefaultValue(0)]
    public LocationStatus Status { get; set; }
    [BsonIgnore]
    public List<Location> SubLocations { get; set; }
    [Field("sub_locations")]
    public List<ObjectId> SubLocationsRef { get; set; }
  }
}
