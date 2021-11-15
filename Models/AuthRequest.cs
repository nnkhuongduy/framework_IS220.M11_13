using System.ComponentModel.DataAnnotations;

namespace _99phantram.Models
{
  public class AuthRequest
  {
    [Required, StringLength(50), EmailAddress]
    public string Email { get; set; }
    [Required, StringLength(50, MinimumLength = 7)]
    public string Password { get; set; }
    [Required]
    public string Remember { get; set; }
  }
}