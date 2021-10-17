using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

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
  public class User
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    [BsonElement("email")]
    public string Email { get; set; }
    [BsonElement("password")]
    public string Password { get; set; }
    [BsonElement("first_name")]
    public string FirstName { get; set; }
    [BsonElement("last_name")]
    public string LastName { get; set; }
    [BsonElement("sex")]
    public Gender Sex { get; set; }
    [BsonElement("address")]
    public string Address { get; set; }
    [BsonElement("phone_number")]
    public string PhoneNumber { get; set; }
    [BsonElement("avatar")]
    public string Avatar { get; set; }
    [BsonElement("oauth")]
    [BsonDefaultValue(false)]
    public bool Oauth { get; set; }
    [BsonElement("oauth_provider")]
    public OAuthProvider OauthProvider { get; set; }
    [BsonElement("role")]
    public Role Role { get; set; }
    [BsonElement("status")]
    [BsonDefaultValue(0)]
    public UserStatus Status { get; set; }
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; }
  }
}