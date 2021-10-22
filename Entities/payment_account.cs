using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Entities;

namespace _99phantram.Entities

{
	public enum Status
	{
		ACTIVE = 1,
		DEACTIVE = 2
	}

	[Collection("payment_account")]
		public class Payment_account : Entity, ICreatedOn, IModifiedOn
		{
			[Field("user")]
			public objectId[] User { get; set; }
			[Field("balance")]
			public long Balance { get; set }
			[Field("withdraw_methods")]
			public ObjectId[] Withdraw_methods { get; set; }
			[Field("modified_on")]
			public DateTime ModifiedOn { get; set; }
			
		}
	}