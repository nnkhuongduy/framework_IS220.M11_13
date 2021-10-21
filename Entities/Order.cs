using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

namespace _99phantram.Entities
{
    public enum OrderStatus
    {
        CREATED,
        PAID,
        DELIVERED,
        DECLINED
    }
    [Collection("orders")]
    public class Order : Entity, ICreatedOn, IModifiedOn
    {
        [Field("buyer")]
        public User Buyer { get; set; }
        [Field("seller")]
        public User Seller { get; set; }
        [Field("supplies")]
        public Supply[] Supplies { get; set; }
        [Field("demands")]
        public Demand[] Demands { get; set; }
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
