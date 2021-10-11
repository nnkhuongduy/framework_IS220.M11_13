using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;

namespace _99phantram.Entities
{
  public class UploadedFile
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    [BsonElement("filename")]
    [BsonRequired]
    public string Filename { get; set; }
    [BsonElement("location")]
    [BsonRequired]
    public string Location { get; set; }
    [BsonElement("type")]
    [BsonRequired]
    public string Type { get; set; }
    [BsonElement("size")]
    [BsonRequired]
    public int Size { get; set; }
    [BsonElement("url")]
    public string Url { get; set; }
    [BsonElement("created_at")]
    [BsonRequired]
    [BsonRepresentation(BsonType.Timestamp)]
    public DateTime CreatedAt { get; set; }
  }
}