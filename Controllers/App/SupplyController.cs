using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using _99phantram.Entities;
using _99phantram.Helpers;
using _99phantram.Interfaces;
using _99phantram.Models;

namespace _99phantram.Controllers
{
  [Route("/api/app/supplies")]
  [ApiController]
  [ServiceFilter(typeof(AppAuthorize))]
  public class SupplyController : ControllerBase
  {
    private readonly ISupplyService _supplyService;

    public SupplyController(ISupplyService supplyService)
    {
      _supplyService = supplyService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Supply>>> GetAllSupplies()
    {
      return await _supplyService.GetAllSupplies();
    }

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Supply>> GetSupply(string id)
    {
      try
      {
        return await _supplyService.GetSupply(id);
      }
      catch (HttpError error)
      {
        return StatusCode(error.Code, error);
      }
    }

    [HttpPut("{id:length(24)}")]
    public async Task<ActionResult> UpdateSupply(string id, PutSupply body)
    {
      try {
        await _supplyService.UpdateSupply(id, body);

        return StatusCode(201);
      } catch (HttpError error)
      {
        return StatusCode(error.Code, error);
      }
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<ActionResult> DeleteService(string id)
    {
      try
      {
        await _supplyService.DeleteSupply(id);

        return StatusCode(204);
      }
      catch (HttpError error)
      {
        return StatusCode(error.Code, error);
      }
    }
  }
}
