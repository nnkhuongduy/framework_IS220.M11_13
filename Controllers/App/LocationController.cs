using System.Collections.Generic;
using System.Threading.Tasks;
using _99phantram.Entities;
using _99phantram.Helpers;
using _99phantram.Interfaces;
using _99phantram.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace _99phantram.Controllers.Apps
{
  [Route("/api/app/categories")]
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
      return await DB.Find<Location>().Match(_ => true).ExecuteAsync();
    }

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Location>> GetLocation(string id)
    {
      try
      {
        var location = await _locationService.GetLocation(id);

        return location;
      }
      catch (HttpError error)
      {

        return NotFound(error);
      }
    }

    [HttpPost]
    public async Task<ActionResult> CreateLocation(LocationBody body)
    {
      await _locationService.CreateLocation(body);

      return StatusCode(201);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<ActionResult<Location>> UpdateLocation(LocationBody body, string id)
    {
      try
      {
        var newLocation = await _locationService.UpdateLocation(body, id);

        if (newLocation.Status == LocationStatus.ARCHIVED)
        {
          await _locationService.ArchiveLocation(newLocation);
        }

        return StatusCode(204);
      }
      catch (HttpError error)
      {
        return NotFound(error);
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
        return NotFound(error);
      }
    }
  }
}