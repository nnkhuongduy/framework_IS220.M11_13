using System.Threading.Tasks;
using MongoDB.Entities;

using _99phantram.Entities;
using _99phantram.Interfaces;
using _99phantram.Models;

namespace _99phantram.Services
{
  public class RatingService : IRatingService
  {
    public async Task<Rating> CreateRating(User user, PostRatingBody body)
    {
      var targetUser = await DB.Find<User>().MatchID(body.RatingOnId).ExecuteFirstAsync();
      var order = await DB.Find<Entities.Order>().MatchID(body.OrderId).ExecuteFirstAsync();

      if (targetUser == null)
        throw new HttpError(false, 404, "Không tìm thấy người dùng");

      if (order == null)
        throw new HttpError(false, 404, "Không tìm thấy đơn hàng");

      var supply = await DB.Find<Supply>().MatchID(order.Supply.ID).ExecuteFirstAsync();

      var rating = new Rating();

      rating.UserRef = user;
      rating.RatingOnRef = targetUser;
      rating.SupplyRef = supply;
      rating.OrderRef = order;
      rating.Point = body.Point;
      rating.Comment = body.Comment;

      await rating.SaveAsync();

      return rating;
    }

    public async Task<Rating> GetRating(string id)
    {
      var rating = await DB
        .Find<Rating>()
        .MatchID(id)
        .ExecuteFirstAsync();

      if (rating == null)
        throw new HttpError(false, 404, "Không tìm thấy đánh giá!");

      var userSnapshot = new UserSnapshot();
      var user = await rating.UserRef.ToEntityAsync();
      var supplySnapshot = new SupplySnapshot();
      var supply = await rating.SupplyRef.ToEntityAsync();

      userSnapshot.ID = user.ID;
      userSnapshot.FirstName = user.FirstName;
      userSnapshot.LastName = user.LastName;
      userSnapshot.Avatar = user.Avatar;

      supplySnapshot.Name = supply.Name;

      rating.User = userSnapshot;
      rating.Supply = supplySnapshot;

      return rating;
    }
  }
}