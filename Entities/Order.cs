using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

using _99phantram.Models;

namespace _99phantram.Entities
{
  public enum OrderStatus
  {
    CREATED,
    CONFIRMING,
    PAID,
    DELIVERED,
    DECLINED
  }
  [Collection("orders")]
  public class Order : Entity, ICreatedOn
  {
    [Field("buyer")]
    public UserSnapshot Buyer { get; set; }
    [Field("seller")]
    public UserSnapshot Seller { get; set; }
    [Field("supply")]
    public SupplySnapshot Supply { get; set; }
    [Field("demand")]
    public Demand Demand { get; set; }
    [Field("amount")]
    public long Amount { get; set; }
    [Field("status")]
    [BsonDefaultValue(0)]
    public OrderStatus Status { get; set; }
    [Field("created_on")]
    public DateTime CreatedOn { get; set; }
    [Field("paid_on")]
    public DateTime PaidOn { get; set; }
  }
}
