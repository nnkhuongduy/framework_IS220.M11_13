using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using MongoDB.Entities;

using _99phantram.Interfaces;
using _99phantram.Entities;
using _99phantram.Helpers;
using _99phantram.Models;

namespace _99phantram.Controllers
{
  [Route("/api/app/locations")]
  [ApiController]
  [ServiceFilter(typeof(AppAuthorize))]
  public class LocationController : ControllerBase
  {
    private readonly ILocationService _locationService;

    public LocationController(ILocationService locationService)
    {
      _locationService = locationService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Location>>> GetAllLocations()
    {
      return await DB
        .Find<Location>()
        .Match(_ => true)
        .Sort(_ => _.ID, MongoDB.Entities.Order.Descending)
        .ExecuteAsync();
    }

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Location>> GetLocation(string id)
    {
      try
      {
        return await _locationService.GetLocation(id);
      }
      catch (HttpError error)
      {
        return StatusCode(error.Code, error);
      }
    }

    [HttpPost]
    public async Task<ActionResult> CreateLocation(LocationBody body)
    {
      await _locationService.CreateLocation(body);

      return StatusCode(204);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<ActionResult> UpdateLocation(string id, LocationBody body)
    {
      try
      {
        var location = await _locationService.UpdateLocation(id, body);

        return StatusCode(204);
      }
      catch (HttpError error)
      {
        return StatusCode(error.Code, error);
      }
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<ActionResult<Location>> DeleteLocation(string id)
    {
      try
      {
        await _locationService.DeleteLocation(id);

        return StatusCode(204);
      }
      catch (HttpError error)
      {
        return StatusCode(error.Code, error);
      }
    }
  }
}