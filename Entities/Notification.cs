using System;
using MongoDB.Bson;
using MongoDB.Entities;

namespace _99phantram.Entities
{
  public enum NotificationLevel
  {
    SUCCESS,
    WARNING,
    ADANGER
  }

  [Collection("notifications")]
  public class Notification : Entity, ICreatedOn
  {
    [Field("user")]
    public ObjectId[] User { get; set; }
    [Field("content")]
    public string Content { get; set; }
    [Field("notification_level")]
    public NotificationLevel Notification_level { get; set; }
    [Field("seen")]
    public bool Seen { get; set; }
    [Field("created_on")]
    public DateTime CreatedOn { get; set; }
  }
}
