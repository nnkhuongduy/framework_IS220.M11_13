using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Entities;

namespace _99phantram.Entities

{
	public enum Status
	{
		CREATED = 1,
		ACTIVE = 2,
		SOLD = 3,
		REVIEWING = 4,
		ARCHIVED = 5
	}
	
	public enum Product_status 
	{
		99 = 1,
		90 = 2,
		80 = 3,
		50 = 4
	}

	[Collection("supplies")]
		public class Supplies : Entity, ICreatedOn, IModifiedOn
		{
			[Field("owner")]
			public Users Owner { get; set; }
			[Field("name")]
			public string Name { get; set; }
			[Field("price")]
			public long Price { get; set }
			[Field("description")]
			public string Description { get; set; }
			[Field("service")]
			public Service[] Services { get; set; }
			[Field("specs")]
			public Specs[] Specs { get; set; }
			[Field("images")]
			public string[] Images { get; set; }
			[Field("categories")]
			public Categories[] Categories { get; set; }
			[Field("locations")]
			public Location[] Locations { get; set; }
			[Field("product_status")]
			public Product_status Product_status { get; set; }
			[Field("status")]
			public Status Status { get; set; }
			[Field("created_on")]
			public DateTime CreatedOn { get; set; }
			[Field("modified_on")]
			public DateTime ModifiedOn { get; set; }
		}
	}