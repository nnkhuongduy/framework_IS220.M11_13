using _99phantram.Entities;

namespace _99phantram.Models
{
  public class AuthResponse
  {
    public User Identifier { get; set; }
    public string Token { get; set; }
    public AuthResponse(User identifier, string token)
    {
      Identifier = identifier;
      Token = token;
    }
  }
}