using System.Threading.Tasks;

using _99phantram.Entities;

namespace _99phantram.Interfaces
{
  public interface IChatClient
  {
    Task ReceiveMessage(ChatMessage message);
    Task JoinGroup(string groupName);
    Task LeaveGroup(string groupName);
  }
}