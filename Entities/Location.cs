using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;
using MongoDB.Bson;

namespace _99phantram.Entities
{
    public enum LocationLevel
    {
        PROVINCE,
        CITY,
        DISTRICT
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
        public LocationLevel Locationlevel { get; set; }
        [Field("status")]
        [BsonDefaultValue(0)]
        public LocationStatus Status { get; set; }
        [Field("sub_locations")]
        public ObjectId[] SubLocations { get; set; }
    }
}
