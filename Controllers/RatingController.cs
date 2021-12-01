using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

using _99phantram.Interfaces;
using _99phantram.Models;
using _99phantram.Helpers;
using _99phantram.Entities;

namespace _99phantram.Controllers
{
  [Route("/api/rating")]
  [ApiController]
  [ServiceFilter(typeof(ClientAuthorize))]
  public class ClientRatingController : ControllerBase
  {
    private readonly IRatingService _ratingService;
    private readonly JwtHolder _jwtHolder;

    public ClientRatingController(IRatingService ratingService, JwtHolder jwtHolder)
    {
      _ratingService = ratingService;
      _jwtHolder = jwtHolder;
    }

    [HttpPost("create")]
    public async Task<ActionResult<Rating>> CreateRating(PostRatingBody body)
    {
      try
      {
        return await _ratingService.CreateRating(_jwtHolder.User, body);
      }
      catch (HttpError error)
      {
        return StatusCode(error.Code, error);
      }
    }

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Rating>> GetRating(string id)
    {
      try
      {
        return await _ratingService.GetRating(id);
      }
      catch (HttpError error)
      {
        return StatusCode(error.Code, error);
      }
    }
  }
}