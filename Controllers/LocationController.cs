using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using MongoDB.Entities;

using _99phantram.Interfaces;
using _99phantram.Entities;
using _99phantram.Models;

namespace _99phantram.Controllers
{
  [Route("/api/location")]
  [ApiController]
  public class ClientLocationController : ControllerBase
  {
    private readonly ILocationService _locationService;
    public ClientLocationController(ILocationService locationService)
    {
      _locationService = locationService;
    }

    [HttpGet("provinces")]
    public async Task<ActionResult<List<Location>>> GetAllProvinces()
    {
      return await DB.Find<Location>().Match(_ => _.LocationLevel == LocationLevel.PROVINCE && _.Status != LocationStatus.ARCHIVED).ExecuteAsync();
    }

    [HttpGet("wards/{id:length(24)}")]
    public async Task<ActionResult<List<Location>>> GetAllWards(string id)
    {
      try
      {
        var province = await _locationService.GetLocation(id);

        var wards = await DB.Find<Location>().Match(_ => _.In("_id", province.SubLocationsRef)).ExecuteAsync();

        return wards;
      }
      catch (HttpError error)
      {
        return StatusCode(error.Code, error);
      }
    }

    [HttpGet("blocks/{id:length(24)}")]
    public async Task<ActionResult<List<Location>>> GetAllBlocks(string id)
    {
      try
      {
        var ward = await _locationService.GetLocation(id);

        var blocks = await DB.Find<Location>().Match(_ => _.In("_id", ward.SubLocationsRef)).ExecuteAsync();

        return blocks;
      }
      catch (HttpError error)
      {
        return StatusCode(error.Code, error);
      }
    }
  }
}