using _99phantram.Entities;

namespace _99phantram.Models
{
  public class AuthResponse
  {
    public UserBase Identifier { get; set; }
    public string Token { get; set; }


    public AuthResponse(UserBase identifier, string token)
    {
      Identifier = identifier;
      Token = token;
    }
  }
}