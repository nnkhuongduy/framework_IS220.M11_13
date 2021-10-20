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

	[Collection("service_type")]
		public class service_type : Entity, ICreatedOn, IModifiedOn
		{
			[Field("name")]
			public ObjectId[] Name { get; set; }
			[Field("status")]
			public status status { get; set; }
			[Field("Value")]
			public object Value { get; set }
			
		}
	}