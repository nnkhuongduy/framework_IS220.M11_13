using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

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
        [Field("service_type")]
        public service_type ServiceType { get; set; }
        [Field("value")]
        public object Value { get; set; }
        [Field("status")]
        [BsonDefaultValue(0)]
        public ServiceStatus Status { get; set; }
        [Field("created_on")]
        public DateTime CreatedOn { get; set; }
        [Field("modified_on")]
        public DateTime ModifiedOn { get; set; }
    }
}
