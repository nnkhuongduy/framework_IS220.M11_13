using System;
using System.ComponentModel.DataAnnotations;

using _99phantram.Entities;

namespace _99phantram.Models
{
  public class StartChatBody
  {
    [Required]
    [StringLength(24)]
    public string ReceiverId { get; set; }

    [Required]
    public string Message { get; set; }
  }

  public class ChatListResponse
  {
    public string ID { get; set; }

    public UserMiniResponse User { get; set; }

    public ChatMessage LastMessage { get; set; }

    public int Unseens { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime ModifiedOn { get; set; }
  }

  public class SendMessageBody
  {
    [Required]
    public string Message { get; set; }
  }
}