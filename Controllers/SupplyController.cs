using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

using _99phantram.Interfaces;
using _99phantram.Entities;
using _99phantram.Models;
using _99phantram.Helpers;

namespace _99phantram.Controllers
{
  [Route("/api/supply")]
  [ApiController]
  public class ClientSupplyController : ControllerBase
  {
    private readonly ISupplyService _supplyService;
    private readonly JwtHolder _jwtHolder;
    public ClientSupplyController(ISupplyService supplyService, JwtHolder jwtHolder)
    {
      _supplyService = supplyService;
      _jwtHolder = jwtHolder;
    }

    [HttpPost]
    [ServiceFilter(typeof(ClientAuthorize))]
    public async Task<ActionResult<Supply>> CreateSupply(ClientPostSupply body)
    {
      try
      {
        var supply = await _supplyService.CreateSupply(_jwtHolder.User, body);

        return StatusCode(201);
      }
      catch (HttpError error)
      {
        return StatusCode(error.Code, error);
      }
    }

    [HttpGet]
    public async Task<ActionResult<List<Supply>>> GetActiveSupplies([FromQuery] SupplyQueryFilter query)
    {
      return await _supplyService.GetActiveSupplies(query);
    }

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Supply>> GetSupply(string id)
    {
      return await _supplyService.GetSupply(id);
    }

    [HttpGet("management")]
    [ServiceFilter(typeof(ClientAuthorize))]
    public async Task<ActionResult<List<Supply>>> GetOwnSupplies()
    {
      var user = _jwtHolder.User;

      return await _supplyService.GetOwnSupplies(user);
    }
  }
}