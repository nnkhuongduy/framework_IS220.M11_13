using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Entities;

namespace _99phantram.Entities

{
	public enum SupplyStatus
	{
		CREATED,
		ACTIVE,
		SOLD,
		REVIEWING,
		ARCHIVED
	}

	[Collection("supplies")]
		public class Supply : Entity, ICreatedOn, IModifiedOn
		{
			[Field("owner")]
			public ObjectId Owner { get; set; }
			[Field("name")]
			public string Name { get; set; }
			[Field("price")]
			public long Price { get; set; }
			[Field("description")]
			public string Description { get; set; }
			[Field("service")]
			public Service[] Services { get; set; }
			[Field("specs")]
			public Spec[] Specs { get; set; }
			[Field("images")]
			public string[] Images { get; set; }
			[Field("categories")]
			public Category[] Categories { get; set; }
			[Field("locations")]
			public Location[] Locations { get; set; }
			[Field("product_status")]
			public ProductStatus Product_status { get; set; }
			[Field("status")]
			public SupplyStatus Status { get; set; }
			[Field("created_on")]
			public DateTime CreatedOn { get; set; }
			[Field("modified_on")]
			public DateTime ModifiedOn { get; set; }
		}
	}