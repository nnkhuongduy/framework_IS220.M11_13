using System.Threading.Tasks;

using _99phantram.Entities;
using _99phantram.Models;

namespace _99phantram.Interfaces
{
  public interface IRatingService
  {
    Task<Rating> CreateRating(User user, PostRatingBody body);
    Task<Rating> GetRating(string id);
  }
}