using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

using _99phantram.Models;

namespace _99phantram.Entities
{

  [Collection("chats")]
  public class Chat : Entity, ICreatedOn, IModifiedOn
  {
    [BsonIgnore]
    public UserMiniResponse User1 { get; set; }

    [Field("user1")]
    public One<User> User1Ref { get; set; }

    [BsonIgnore]
    public UserMiniResponse User2 { get; set; }

    [Field("user2")]
    public One<User> User2Ref { get; set; }

    [Field("content")]
    public List<ChatMessage> Content { get; set; }

    [Field("created_on")]
    public DateTime CreatedOn { get; set; }

    [Field("modified_on")]
    public DateTime ModifiedOn { get; set; }

  }
}
