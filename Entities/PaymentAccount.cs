using System;
using MongoDB.Bson;
using MongoDB.Entities;

namespace _99phantram.Entities
{
  public enum Status
  {
    ACTIVE,
    DEACTIVE
  }

  [Collection("payment_account")]
  public class PaymentAccount : Entity, IModifiedOn
  {
    [Field("user")]
    public ObjectId User { get; set; }
    [Field("balance")]
    public long Balance { get; set; }
    [Field("withdraw_methods")]
    public object[] WithdrawMethods { get; set; }
    [Field("modified_on")]
    public DateTime ModifiedOn { get; set; }

  }
}
