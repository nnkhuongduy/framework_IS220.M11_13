using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Entities;

namespace _99phantram.Entities

{
	public enum Notification_level
	{
		SUCCESS = 1,
		WARNING = 2,
		ADANGER = 3
	}

	[Collection("notifications")]
		public class Notifications : Entity, ICreatedOn, IModifiedOn
		{
			[Field("user")]
			public objectId[] user { get; set; }
			[Field("content")]
			public string Name { get; set; }
			[Field("notification_level")]
			public Notification_level Notification_level { get; set; }
			[Field("seen")]
			[BsonDefaultValue(false)]
			public bool Seen { get; set; }
			[Field("created_on")]
			public DateTime CreatedOn { get; set; }
		}
	}