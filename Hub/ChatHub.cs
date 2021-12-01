using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

using _99phantram.Interfaces;

namespace _99phantram.Hubs
{
  public class ChatHub : Hub<IChatClient>
  {
    public Task JoinGroup(string groupName)
    {
      return Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public Task LeaveGroup(string groupName)
    {
      return Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }
  }
}