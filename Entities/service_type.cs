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

	[Collection("service_type")]
		public class Service_type : Entity, ICreatedOn, IModifiedOn
		{
			[Field("name")]
			public ObjectId[] Name { get; set; }
			[Field("status")]
			public Status Status { get; set; }
			[Field("Value")]
			public object Value { get; set }
			
		}
	}