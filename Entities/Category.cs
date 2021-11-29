using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;
using MongoDB.Bson;
using System.Collections.Generic;

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
    [Field("image")]
    public string Image { get; set; }
    [Field("category_level")]
    [BsonDefaultValue(0)]
    public CategoryLevel CategoryLevel { get; set; }
    [Field("status")]
    [BsonDefaultValue(0)]
    public CategoryStatus Status { get; set; }
    [Field("specs")]
    public List<Spec> Specs { get; set; }
    [Field("sub_categories")]
    public List<ObjectId> SubCategories { get; set; }
    [Field("slug")]
    public string Slug {get; set;}
  }
}
