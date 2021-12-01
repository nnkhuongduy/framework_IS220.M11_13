using System.Collections.Generic;
using System.Threading.Tasks;

using _99phantram.Entities;
using _99phantram.Models;

namespace _99phantram.Interfaces
{
  public interface IChatService
  {
    Task<Chat> StartChat(StartChatBody body, User sender);
    Task<List<ChatListResponse>> GetChats(User user);
    Task<Chat> ConnectChat(User user, string chatId);
    Task DisconnectChat(User user, string chatId);
    Task<ChatMessage> SendMessage(SendMessageBody body, User user, string chatId);
  }
}