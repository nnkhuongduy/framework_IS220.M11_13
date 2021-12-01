using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

using _99phantram.Interfaces;
using _99phantram.Models;
using _99phantram.Helpers;
using _99phantram.Entities;

namespace _99phantram.Controllers
{
  [Route("/api/order")]
  [ApiController]
  [ServiceFilter(typeof(ClientAuthorize))]
  public class ClientOrderController : ControllerBase
  {
    private readonly IOrderService _orderService;
    private readonly JwtHolder _jwtHolder;

    public ClientOrderController(IOrderService orderService, JwtHolder jwtHolder)
    {
      _orderService = orderService;
      _jwtHolder = jwtHolder;
    }

    [HttpPost("create")]
    public async Task<ActionResult<Entities.Order>> CreateOrder(PostOrderBody body)
    {
      try
      {
        return await _orderService.CreateOrder(_jwtHolder.User, body);
      }
      catch (HttpError error)
      {
        return StatusCode(error.Code, error);
      }
    }

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Entities.Order>> GetOrder(string id)
    {
      try
      {
        var user = _jwtHolder.User;
        var order = await _orderService.ClientGetOrder(user, id);

        if (order.Seller.ID != user.ID && order.Buyer.ID != user.ID)
          throw new HttpError(false, 400, "Không thể lấy thông tin hóa đơn này!");

        return order;
      }
      catch (HttpError error)
      {
        return StatusCode(error.Code, error);
      }
    }

    [HttpPut("paid/{id:length(24)}")]
    public async Task<ActionResult<Order>> PaidOrder(PutOrderBody body, string id)
    {
      try
      {
        return await _orderService.PaidOrder(_jwtHolder.User, body, id);
      }
      catch (HttpError error)
      {
        return StatusCode(error.Code, error);
      }
    }

    [HttpPut("confirm/{id:length(24)}")]
    public async Task<ActionResult<Order>> ConfirmOrder(PutOrderBody body, string id)
    {
      try
      {
        return await _orderService.ConfirmOrder(_jwtHolder.User, body, id);
      }
      catch (HttpError error)
      {
        return StatusCode(error.Code, error);
      }
    }

    [HttpPut("received/{id:length(24)}")]
    public async Task<ActionResult<Order>> ReceivedSupply(PutOrderBody body, string id)
    {
      try
      {
        return await _orderService.ReceivedSupply(_jwtHolder.User, body, id);
      }
      catch (HttpError error)
      {
        return StatusCode(error.Code, error);
      }
    }
  }
}