using System.Threading.Tasks;
using MongoDB.Entities;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.SignalR;

using _99phantram.Entities;
using _99phantram.Interfaces;
using _99phantram.Models;
using _99phantram.Hubs;

namespace _99phantram.Services
{
  public class ChatService : IChatService
  {
    private readonly IHubContext<ChatHub, IChatClient> _chatHub;
    private readonly Dictionary<string, List<string>> _connectingUsers = new Dictionary<string, List<string>>();

    public ChatService(IHubContext<ChatHub, IChatClient> chatHub)
    {
      _chatHub = chatHub;
    }

    private void _connect(string userId, string chatId)
    {
      if (!_connectingUsers.ContainsKey(userId))
        _connectingUsers.Add(userId, new List<string>());

      if (!_connectingUsers[userId].Exists(_ => _ == chatId))
        _connectingUsers[userId].Add(chatId);
    }

    private void _disconnect(string userId, string chatId)
    {
      _connectingUsers[userId].Remove(chatId);
    }

    public async Task<Chat> StartChat(StartChatBody body, User sender)
    {
      var chat = await DB
        .Find<Chat>()
        .Match(_ =>
          (_.User1Ref.ID == sender.ID && _.User2Ref.ID == body.ReceiverId) ||
          (_.User1Ref.ID == body.ReceiverId && _.User2Ref.ID == sender.ID)
        )
        .ExecuteFirstAsync();

      ChatMessage chatMessage = new ChatMessage();

      chatMessage.Sender = sender.ID;
      chatMessage.Content = body.Message;
      chatMessage.Seen = false;
      chatMessage.MessageType = ChatMessageType.TEXT;
      chatMessage.CreatedOn = System.DateTime.UtcNow;
      chatMessage.ModifiedOn = System.DateTime.UtcNow;

      if (chat != null)
      {
        chat.Content.Add(chatMessage.ToDocument());

        await chat.SaveAsync();

        return chat;
      }

      var receiver = await DB.Find<User>().MatchID(body.ReceiverId).ExecuteFirstAsync();

      if (receiver == null)
        throw new HttpError(false, 404, "Không tìm thấy người nhận!");

      if (receiver.ID == sender.ID)
        throw new HttpError(false, 400, "Không thể gửi tin nhắn đến bản thân!");

      Chat newChat = new Chat();
      var chatMessages = new List<ChatMessage>();

      chatMessages.Add(chatMessage.ToDocument());

      newChat.User1Ref = sender;
      newChat.User2Ref = receiver;
      newChat.Content = chatMessages;

      await newChat.SaveAsync();

      await _chatHub.Groups.AddToGroupAsync(sender.ID, newChat.ID);
      await _chatHub.Groups.AddToGroupAsync(receiver.ID, newChat.ID);

      return newChat;
    }

    public async Task<List<ChatListResponse>> GetChats(User user)
    {
      var chats = await DB
        .Find<Chat>()
        .Match(_ => (_.User1Ref.ID == user.ID || _.User2Ref.ID == user.ID) && _.Content.Count > 0)
        .ExecuteAsync();
      var responses = new List<ChatListResponse>();

      foreach (var chat in chats)
      {
        var response = new ChatListResponse();
        var userResponse = new UserMiniResponse();
        var targetUserRef = chat.User1Ref.ID == user.ID ? chat.User2Ref : chat.User1Ref;
        var targetUser = await targetUserRef.ToEntityAsync();

        userResponse.ID = targetUser.ID;
        userResponse.FirstName = targetUser.FirstName;
        userResponse.LastName = targetUser.LastName;
        userResponse.Avatar = targetUser.Avatar;

        response.ID = chat.ID;
        response.User = userResponse;
        response.LastMessage = chat.Content[chat.Content.Count - 1];
        response.Unseens = chat.Content.Where(_ => _.Sender == targetUser.ID && _.Seen == false).ToList().Count;
        response.CreatedOn = chat.CreatedOn;
        response.ModifiedOn = chat.ModifiedOn;

        responses.Add(response);
      }

      return responses;
    }

    public async Task<Chat> ConnectChat(User user, string chatId)
    {
      var chat = await DB.Find<Chat>().MatchID(chatId).ExecuteFirstAsync();

      if (chat == null)
        throw new HttpError(false, 404, "Chi tiết liên hệ không tìm thấy!");

      if (chat.User1Ref.ID != user.ID && chat.User2Ref.ID != user.ID)
        throw new HttpError(false, 400, "Không thể gửi tin nhắn đến người này!");

      foreach (var content in chat.Content)
      {
        if (content.Sender != user.ID)
          content.Seen = true;
      }

      await chat.SaveAsync();

      var user1 = await DB
        .Find<User, UserMiniResponse>()
        .MatchID(chat.User1Ref.ID)
        .Project(_ => new UserMiniResponse { ID = _.ID, FirstName = _.FirstName, LastName = _.LastName, Avatar = _.Avatar })
        .ExecuteFirstAsync();
      var user2 = await DB
        .Find<User, UserMiniResponse>()
        .MatchID(chat.User2Ref.ID)
        .Project(_ => new UserMiniResponse { ID = _.ID, FirstName = _.FirstName, LastName = _.LastName, Avatar = _.Avatar })
        .ExecuteFirstAsync();

      chat.User1 = user1;
      chat.User2 = user2;

      _connect(user.ID, chat.ID);

      return chat;
    }

    public async Task DisconnectChat(User user, string chatId)
    {
      var chat = await DB.Find<Chat>().MatchID(chatId).ExecuteFirstAsync();

      if (chat == null)
        throw new HttpError(false, 404, "Chi tiết liên hệ không tìm thấy!");

      if (chat.User1Ref.ID != user.ID && chat.User2Ref.ID != user.ID)
        throw new HttpError(false, 400, "Không thể gửi tin nhắn đến người này!");

      _disconnect(user.ID, chat.ID);
    }

    public async Task<ChatMessage> SendMessage(SendMessageBody body, User user, string chatId)
    {
      var chat = await DB.Find<Chat>().MatchID(chatId).ExecuteFirstAsync();

      if (chat == null)
        throw new HttpError(false, 404, "Chi tiết liên hệ không tìm thấy!");

      if (chat.User1Ref.ID != user.ID && chat.User2Ref.ID != user.ID)
        throw new HttpError(false, 400, "Không thể gửi tin nhắn đến người này!");

      var chatMessage = new ChatMessage();

      chatMessage.Sender = user.ID;
      chatMessage.Content = body.Message;
      chatMessage.Seen = _connectingUsers[user.ID].Exists(_ => _ == chat.ID);
      chatMessage.MessageType = ChatMessageType.TEXT;
      chatMessage.CreatedOn = System.DateTime.UtcNow;
      chatMessage.ModifiedOn = System.DateTime.UtcNow;

      var chatDocument = chatMessage.ToDocument();

      chat.Content.Add(chatDocument);

      await chat.SaveAsync();
      await _chatHub.Clients.Group(chat.ID).ReceiveMessage(chatDocument);

      _connect(user.ID, chat.ID);

      return chatDocument;
    }
  }
}