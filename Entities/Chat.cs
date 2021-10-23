using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Entities;

namespace _99phantram.Entities
{

[Collection("chats")]
  public class Chat : Entity, ICreatedOn, IModifiedOn
  {
    [Field("user1")]
    public ObjectId User1 { get; set; }
    [Field("user2")]
    public ObjectId User2 { get; set; }
    [Field("content")]
    public ChatMessage[] Content { get; set; }
    [Field("created_on")]
    public DateTime CreatedOn { get; set; }
    [Field("modified_on")]
    public DateTime ModifiedOn { get; set; }
  }
}
