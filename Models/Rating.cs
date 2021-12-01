using System.ComponentModel.DataAnnotations;

namespace _99phantram.Models
{
  public class PostRatingBody
  {
    [Required]
    [StringLength(24)]
    public string RatingOnId { get; set; }
    [Required]
    [StringLength(24)]
    public string OrderId { get; set; }
    [Required]
    [Range(0, 5)]
    public int Point { get; set; }
    [Required]
    [MaxLength(100)]
    public string Comment { get; set; }
  }
}