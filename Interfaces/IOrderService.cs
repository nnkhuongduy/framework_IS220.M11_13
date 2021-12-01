using System.Threading.Tasks;

using _99phantram.Entities;
using _99phantram.Models;

namespace _99phantram.Interfaces
{
  public interface IOrderService
  {
    Task<Order> CreateOrder(User user, PostOrderBody body);
    Task<Order> ClientGetOrder(User user, string id);
    Task<Order> PaidOrder(User user, PutOrderBody body, string orderId);
    Task<Order> ConfirmOrder(User user, PutOrderBody body, string orderId);
    Task<Order> ReceivedSupply(User user, PutOrderBody body, string orderId);
  }
}