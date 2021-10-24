using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;
using MongoDB.Bson

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
        public ObjectId ServiceType { get; set; }
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
