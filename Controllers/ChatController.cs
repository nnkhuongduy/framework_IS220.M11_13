using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

using _99phantram.Interfaces;
using _99phantram.Models;
using _99phantram.Helpers;
using _99phantram.Hubs;
using _99phantram.Entities;

namespace _99phantram.Controllers
{
  [Route("/api/chat")]
  [ApiController]
  [ServiceFilter(typeof(ClientAuthorize))]
  public class ClientChatController : ControllerBase
  {
    private readonly IChatService _chatService;
    private readonly JwtHolder _jwtHolder;

    public ClientChatController(IChatService chatService, JwtHolder jwtHolder)
    {
      _chatService = chatService;
      _jwtHolder = jwtHolder;
    }

    [HttpPost("start")]
    public async Task<ActionResult<Dictionary<string, string>>> StartChat(StartChatBody body)
    {
      try
      {
        var sender = _jwtHolder.User;
        var chat = await _chatService.StartChat(body, sender);
        var response = new Dictionary<string, string>();

        response.Add("chatId", chat.ID);

        return response;
      }
      catch (HttpError error)
      {
        return StatusCode(error.Code, error);
      }
    }

    [HttpGet("list")]
    public async Task<ActionResult<List<ChatListResponse>>> GetChats()
    {
      var user = _jwtHolder.User;
      var chats = await _chatService.GetChats(user);

      return chats;
    }

    [HttpGet("connect/{id:length(24)}")]
    public async Task<ActionResult<Chat>> ConnectChat(string id)
    {
      try
      {
        var user = _jwtHolder.User;

        return await _chatService.ConnectChat(user, id);
      }
      catch (HttpError error)
      {
        return StatusCode(error.Code, error);
      }
    }

    [HttpGet("disconnect/{id:length(24)}")]
    public async Task<ActionResult> DisconnectChat(string id)
    {
      try
      {
        var user = _jwtHolder.User;

        await _chatService.DisconnectChat(user, id);

        return StatusCode(200);
      }
      catch (HttpError error)
      {
        return StatusCode(error.Code, error);
      }
    }

    [HttpPost("send/{id:length(24)}")]
    public async Task<ActionResult<ChatMessage>> SendMessage(string id, SendMessageBody body)
    {
      try
      {
        var user = _jwtHolder.User;
        var message = await _chatService.SendMessage(body, user, id);

        return message;
      }
      catch (HttpError error)
      {
        return StatusCode(error.Code, error);
      }
    }
  }
}