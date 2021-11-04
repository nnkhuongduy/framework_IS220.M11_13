using _99phantram.Entities;

namespace _99phantram.Models
{
  public class JwtHolder
  {
    public string Token { get; set; }
    public User User {get; set;}

    public JwtHolder() { }
  }
}