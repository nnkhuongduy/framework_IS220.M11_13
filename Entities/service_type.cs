using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Entities;

namespace _99phantram.Entities

{
	public enum ServiceTypeStatus
	{
		ACTIVE,
		DEACTIVE
	}

	[Collection("service_types")]
		public class ServiceType : Entity
		{
			[Field("name")]
			public string Name { get; set; }
			[Field("status")]
			public ServiceTypeStatus Status { get; set; }
			[Field("value")]
			public object Value { get; set; }
			
		}
	}