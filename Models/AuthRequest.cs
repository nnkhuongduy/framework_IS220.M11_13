using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace _99phantram.Models
{
  public class AuthRequest
  {
    [Required, StringLength(50)]
    public string Username { get; set; }
    [Required, StringLength(50, MinimumLength = 7)]
    public string Password { get; set; }
    [Required]
    public string Remember { get; set; }

    public AuthRequest(string username, string password, string rememer)
    {
      Username = username;
      Password = password;
      Remember = rememer;
    }
  }
}