using System.ComponentModel.DataAnnotations;

namespace _99phantram.Models
{
  public class PostOrderBody
  {
    [Required]
    [StringLength(24)]
    public string SupplyId { get; set; }

    [Required]
    [StringLength(24)]
    public string ChatId { get; set; }
  }

  public class PutOrderBody
  {
    [Required]
    [StringLength(24)]
    public string chatId { get; set; }
  }
}