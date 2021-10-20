using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Entities;

namespace _99phantram.Entities

{
	public enum status
	{
		ACTIVE = 1,
		DEACTIVE = 2
	}

	[Collection("payment_account")]
		public class payment_account : Entity, ICreatedOn, IModifiedOn
		{
			[Field("user")]
			public objectId[] user { get; set; }
			[Field("balance")]
			public long balance { get; set }
			[Field("withdraw_methods")]
			public ObjectId[] withdraw_methods { get; set; }
			[Field("modified_on")]
			public DateTime ModifiedOn { get; set; }
			
		}
	}