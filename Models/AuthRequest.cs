using System.ComponentModel.DataAnnotations;

namespace _99phantram.Models
{
  public class AuthRequest
  {
    [Required, StringLength(50), EmailAddress]
    public string Email { get; set; }
    [Required, StringLength(50, MinimumLength = 7)]
    public string Password { get; set; }
    public string Remember { get; set; }
  }

  public class StepTwoUpdateRequest
  {
    [Required]
    public string PhoneNumber {get; set;}
    [Required]
    public string Province {get; set;}
    [Required]
    public string Ward {get; set;}
    [Required]
    public string Block {get; set;}
    [Required]
    public string Address {get; set;}
  }
}