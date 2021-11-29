using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

namespace _99phantram.Entities
{
  public enum Gender
  {
    Male,
    Female,
    Other
  }
  public enum OAuthProvider
  {
    None,
    Facebook,
    Google
  }
  public enum UserStatus
  {
    CREATED,
    VERIFIED,
    ARCHIVED
  }

  [Collection("users")]
  public class User : Entity, ICreatedOn, IModifiedOn
  {
    [Field("email")]
    public string Email { get; set; }

    [Field("password")]
    public string Password { get; set; }

    [Field("first_name")]
    public string FirstName { get; set; }

    [Field("last_name")]
    public string LastName { get; set; }

    [Field("sex")]
    public Gender Sex { get; set; }

    [Field("address")]
    public string Address { get; set; }

    [BsonIgnore]
    public Location LocationProvince { get; set; }

    [Field("location_provice")]
    public One<Location> LocationProvinceRef { get; set; }

    [BsonIgnore]
    public Location LocationWard { get; set; }

    [Field("location_ward")]
    public One<Location> LocationWardRef { get; set; }

    [BsonIgnore]
    public Location LocationBlock { get; set; }

    [Field("location_block")]
    public One<Location> LocationBlockRef { get; set; }

    [Field("phone_number")]
    public string PhoneNumber { get; set; }

    [Field("avatar")]
    public string Avatar { get; set; }

    [Field("oauth")]
    [BsonDefaultValue(false)]
    public bool Oauth { get; set; }

    [Field("oauth_provider")]
    public OAuthProvider OauthProvider { get; set; }

    [Field("role")]
    public Role Role { get; set; }

    [Field("status")]
    [BsonDefaultValue(0)]
    public UserStatus Status { get; set; }

    [Field("created_on")]
    public DateTime CreatedOn { get; set; }

    [Field("modified_on")]
    public DateTime ModifiedOn { get; set; }

  }
}
