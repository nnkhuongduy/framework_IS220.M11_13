using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Entities;

namespace _99phantram.Entities
{
  public enum RoleLevel
  {
    CLIENT = 1,
    APP = 2,
    ALL = 3
  }
  
  [Collection("roles")]
  public class Role : Entity, ICreatedOn, IModifiedOn
  {
    [Field("name")]
    public string Name { get; set; }
    [Field("role_level")]
    public RoleLevel RoleLevel { get; set; }
    [Field("selectable_roles")]
    public ObjectId[] SelectableRoles { get; set; }
    [Field("created_on")]
    public DateTime CreatedOn { get; set; }
    [Field("modified_on")]
    public DateTime ModifiedOn { get; set; }
  }
}