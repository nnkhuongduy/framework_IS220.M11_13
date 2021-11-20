using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;
using MongoDB.Bson;

namespace _99phantram.Entities
{
    public enum ServiceStatus
    {
        ACTIVE,
        EXPIRED
    }
    [Collection("services")]
    public class Service : Entity, ICreatedOn, IModifiedOn
    {
        [BsonIgnore]
        public ServiceType ServiceType { get; set; }
        
        [Field("name")]
        public string Name { get; set; }
        
        [BsonIgnore]
        public Dictionary<string, object> Value { get; set; }
        
        [JsonIgnore]
        [Field("value")]
        public BsonDocument ValueBson { get; set; }
        
        [Field("status")]
        [BsonDefaultValue(0)]
        public ServiceStatus Status { get; set; }
        
        [Field("created_on")]
        public DateTime CreatedOn { get; set; }
        
        [Field("modified_on")]
        public DateTime ModifiedOn { get; set; }
        
        [Field("service_type")]
        public One<ServiceType> ServiceTypeRef { get; set; }
    }
}
