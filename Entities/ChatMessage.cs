using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

namespace _99phantram.Entities
{
  public enum ChatMessageType
  {
    TEXT,
    REQUEST_PAYMENT,
    CONFIRM_PAYMENT,
    CONFIRM_RECEIVED,
    RATED,
  }

  [Collection("chat_messages")]
  public class ChatMessage : Entity, ICreatedOn, IModifiedOn
  {
    [Field("sender")]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string Sender { get; set; }
    [Field("seen")]
    public bool Seen { get; set; }
    [Field("content")]
    public string Content { get; set; }
    [Field("supply_id")]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string SupplyId { get; set; }
    [Field("order_id")]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string OrderId { get; set; }
    [Field("rating_id")]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string RatingId { get; set; }
    [Field("message_type")]
    public ChatMessageType MessageType { get; set; }
    [Field("created_on")]
    public DateTime CreatedOn { get; set; }
    [Field("modified_on")]
    public DateTime ModifiedOn { get; set; }
  }
}
