using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Entities;

namespace _99phantram.Entities

{
	public enum status
	{
		CREATED = 1,
		ACTIVE = 2,
		SOLD = 3,
		REVIEWING = 4,
		ARCHIVED = 5
	}
	
	public enum product_status 
	{
		99 = 1,
		90 = 2,
		80 = 3,
		50 = 4
	}

	[Collection("supplies")]
		public class supplies : Entity, ICreatedOn, IModifiedOn
		{
			[Field("owner")]
			public users owner { get; set; }
			[Field("name")]
			public string name { get; set; }
			[Field("price")]
			public long price { get; set }
			[Field("description")]
			public string description { get; set; }
			[Field("service")]
			public service[] services { get; set; }
			[Field("specs")]
			public specs[] specs { get; set; }
			[Field("images")]
			public string[] images { get; set; }
			[Field("categories")]
			public categories[] categories { get; set; }
			[Field("locations")]
			public location[] locations { get; set; }
			[Field("product_status")]
			public product_status product_status { get; set; }
			[Field("status")]
			public status status { get; set; }
			[Field("created_on")]
			public DateTime CreatedOn { get; set; }
			[Field("modified_on")]
			public DateTime ModifiedOn { get; set; }
		}
	}