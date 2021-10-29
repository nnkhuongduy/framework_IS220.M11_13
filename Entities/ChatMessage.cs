using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Entities;

namespace _99phantram.Entities
{

[Collection("chat_messages")]
  public class ChatMessage : Entity, ICreatedOn, IModifiedOn
  {
    [Field("sender")]
    public User Sender { get; set; }
    [Field("content")]
    public string Content { get; set; }
    [Field("created_on")]
    public DateTime CreatedOn { get; set; }
    [Field("modified_on")]
    public DateTime ModifiedOn { get; set; }
  }
}
