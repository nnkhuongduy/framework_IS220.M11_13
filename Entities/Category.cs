using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;
using MongoDB.Bson;

namespace _99phantram.Entities
{
    public enum CategoryLevel
    {
        PRIMARY,
        SECONDARY
    }
    public enum CategoryStatus
    {
        NEW,
        ACTIVE,
        ARCHIVED
    }
    [Collection("categories")]
    public class Category : Entity
    {
        [Field("name")]
        public string Name { get; set; }
        [Field("category_level")]
        [BsonDefaultValue(0)]
        public CategoryLevel Categorylevel { get; set; }
        [Field("status")]
        [BsonDefaultValue(0)]
        public CategoryStatus Status { get; set; }
        [Field("specs")]
        public Spec[] Specs { get; set; }
        [Field("sub_categories")]
        public ObjectId[] SubCategories { get; set; }
    }
}
