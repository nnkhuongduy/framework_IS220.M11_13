using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Entities;

using _99phantram.Entities;
using _99phantram.Hubs;
using _99phantram.Interfaces;
using _99phantram.Models;

namespace _99phantram.Services
{
  public class OrderService : IOrderService
  {
    private readonly IHubContext<ChatHub, IChatClient> _chatHub;

    public OrderService(IHubContext<ChatHub, IChatClient> chatHub)
    {
      _chatHub = chatHub;
    }

    public async Task<Entities.Order> CreateOrder(User user, PostOrderBody body)
    {
      var supply = await DB.Find<Supply>().MatchID(body.SupplyId).ExecuteFirstAsync();
      var chat = await DB.Find<Chat>().MatchID(body.ChatId).ExecuteFirstAsync();

      if (supply == null)
        throw new HttpError(false, 404, "Không tìm thấy sản phẩm!");

      if (supply.Status != SupplyStatus.ACTIVE)
        throw new HttpError(false, 400, "Không thể tạo hóa đơn cho sản phẩm này!");

      if (chat == null)
        throw new HttpError(false, 404, "Không tìm thấy thông tin liên hệ");

      var buyer = await (chat.User1Ref.ID == user.ID ? chat.User2Ref : chat.User1Ref).ToEntityAsync(); ;
      var order = new Entities.Order();

      var buyerSnapshot = new UserSnapshot();
      var sellerSnapshot = new UserSnapshot();
      var locationsSnapshot = new List<LocationSnapshot>();
      var supplySnapShot = new SupplySnapshot();

      buyerSnapshot.ID = buyer.ID;
      buyerSnapshot.Email = buyer.Email;
      buyerSnapshot.FirstName = buyer.FirstName;
      buyerSnapshot.LastName = buyer.LastName;

      sellerSnapshot.ID = user.ID;
      sellerSnapshot.Email = user.Email;
      sellerSnapshot.FirstName = user.FirstName;
      sellerSnapshot.LastName = user.LastName;

      locationsSnapshot.Add(new LocationSnapshot()
      {
        Name = supply.Locations[0].Name
      });
      locationsSnapshot.Add(new LocationSnapshot()
      {
        Name = supply.Locations[1].Name
      });
      locationsSnapshot.Add(new LocationSnapshot()
      {
        Name = supply.Locations[2].Name
      });

      supplySnapShot.ID = supply.ID;
      supplySnapShot.Locations = locationsSnapshot;
      supplySnapShot.Name = supply.Name;
      supplySnapShot.Price = supply.Price;
      supplySnapShot.Thumbnail = supply.Thumbnail;

      order.Buyer = buyerSnapshot;
      order.Seller = sellerSnapshot;
      order.Supply = supplySnapShot;
      order.Amount = supplySnapShot.Price;
      order.Status = OrderStatus.CREATED;

      await order.SaveAsync();

      var chatMessage = new ChatMessage();
      chatMessage.Sender = user.ID;
      chatMessage.Seen = false;
      chatMessage.Content = "";
      chatMessage.SupplyId = supply.ID;
      chatMessage.OrderId = order.ID;
      chatMessage.MessageType = ChatMessageType.REQUEST_PAYMENT;
      chatMessage.CreatedOn = System.DateTime.UtcNow;
      chatMessage.ModifiedOn = System.DateTime.UtcNow;

      var chatDocument = chatMessage.ToDocument();

      chat.Content.Add(chatDocument);

      await chat.SaveAsync();
      await _chatHub.Clients.Group(chat.ID).ReceiveMessage(chatDocument);

      return order;
    }

    public async Task<Entities.Order> ClientGetOrder(User user, string orderId)
    {
      var order = await DB.Find<Entities.Order>().MatchID(orderId).ExecuteFirstAsync();

      if (order == null)
        throw new HttpError(false, 404, "Không tìm thấy đơn hàng");

      return order;
    }

    public async Task<Entities.Order> PaidOrder(User user, PutOrderBody body, string orderId)
    {
      var order = await DB.Find<Entities.Order>().MatchID(orderId).ExecuteFirstAsync();
      var chat = await DB.Find<Chat>().MatchID(body.chatId).ExecuteFirstAsync();

      if (order == null)
        throw new HttpError(false, 404, "Không tìm thấy đơn hàng này!");

      if (order.Buyer.ID != user.ID)
        throw new HttpError(false, 400, "Không sở hữu đơn hàng này!");

      if (chat == null)
        throw new HttpError(false, 404, "Không tìm thấy chi tiết liên hệ!");

      order.Status = OrderStatus.CONFIRMING;

      await order.SaveAsync();

      var chatMessage = new ChatMessage();
      chatMessage.Sender = user.ID;
      chatMessage.Seen = false;
      chatMessage.OrderId = order.ID;
      chatMessage.MessageType = ChatMessageType.CONFIRM_PAYMENT;
      chatMessage.CreatedOn = System.DateTime.UtcNow;
      chatMessage.ModifiedOn = System.DateTime.UtcNow;

      var chatDocument = chatMessage.ToDocument();

      chat.Content.Add(chatDocument);

      await chat.SaveAsync();
      await _chatHub.Clients.Group(chat.ID).ReceiveMessage(chatDocument);

      return order;
    }

    public async Task<Entities.Order> ConfirmOrder(User user, PutOrderBody body, string orderId)
    {
      var order = await DB.Find<Entities.Order>().MatchID(orderId).ExecuteFirstAsync();
      var chat = await DB.Find<Chat>().MatchID(body.chatId).ExecuteFirstAsync();
      var supply = await DB.Find<Supply>().MatchID(order.Supply.ID).ExecuteFirstAsync();

      if (order == null)
        throw new HttpError(false, 404, "Không tìm thấy đơn hàng này!");

      if (order.Seller.ID != user.ID)
        throw new HttpError(false, 400, "Không sở hữu đơn hàng này!");

      if (order.Status != OrderStatus.CONFIRMING)
        throw new HttpError(false, 400, "Không thể thanh toán đơn hàng này!");

      if (chat == null)
        throw new HttpError(false, 404, "Không tìm thấy chi tiết liên hệ!");

      if (supply == null)
        throw new HttpError(false, 404, "Không tìm thấy sản phẩm!");

      order.PaidOn = System.DateTime.UtcNow;
      order.Status = OrderStatus.PAID;

      await order.SaveAsync();

      supply.Status = SupplyStatus.SOLD;

      await supply.SaveAsync();

      var chatMessage = new ChatMessage();
      chatMessage.Sender = order.Seller.ID;
      chatMessage.Seen = false;
      chatMessage.OrderId = order.ID;
      chatMessage.MessageType = ChatMessageType.CONFIRM_RECEIVED;
      chatMessage.CreatedOn = System.DateTime.UtcNow;
      chatMessage.ModifiedOn = System.DateTime.UtcNow;

      var chatDocument = chatMessage.ToDocument();

      chat.Content.Add(chatDocument);

      await chat.SaveAsync();
      await _chatHub.Clients.Group(chat.ID).ReceiveMessage(chatDocument);

      return order;
    }

    public async Task<Entities.Order> ReceivedSupply(User user, PutOrderBody body, string orderId)
    {
      var order = await DB.Find<Entities.Order>().MatchID(orderId).ExecuteFirstAsync();
      var chat = await DB.Find<Chat>().MatchID(body.chatId).ExecuteFirstAsync();
      var rating = await DB.Find<Rating>().Match(_ => _.OrderRef.ID == order.ID).ExecuteFirstAsync();

      if (order == null)
        throw new HttpError(false, 404, "Không tìm thấy đơn hàng này!");

      if (order.Buyer.ID != user.ID)
        throw new HttpError(false, 400, "Không sở hữu đơn hàng này!");

      if (order.Status != OrderStatus.PAID)
        throw new HttpError(false, 400, "Đơn hàng chưa thanh toán");

      if (chat == null)
        throw new HttpError(false, 404, "Không tìm thấy chi tiết liên hệ!");

      if (rating == null)
        throw new HttpError(false, 404, "Không tìm thấy đánh giá!");

      order.Status = OrderStatus.DELIVERED;

      await order.SaveAsync();

      var chatMessage = new ChatMessage();
      chatMessage.Sender = order.Buyer.ID;
      chatMessage.Seen = false;
      chatMessage.RatingId = rating.ID;
      chatMessage.MessageType = ChatMessageType.RATED;
      chatMessage.CreatedOn = System.DateTime.UtcNow;
      chatMessage.ModifiedOn = System.DateTime.UtcNow;

      var chatDocument = chatMessage.ToDocument();

      chat.Content.Add(chatDocument);

      await chat.SaveAsync();
      await _chatHub.Clients.Group(chat.ID).ReceiveMessage(chatDocument);


      return order;
    }
  }
}