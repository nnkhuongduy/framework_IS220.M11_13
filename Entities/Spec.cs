using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

namespace _99phantram.Entities
{
    [Collection("specs")]
    public class Spec : Entity
    {
        [Field("name")]
        public string Name { get; set; }
        [Field("value")]
        public string Value { get; set; }
        [Field("required")]
        [BsonDefaultValue(false)]
        public bool Required { get; set; }
    }
}
