using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using _99phantram.Entities;
using _99phantram.Helpers;
using _99phantram.Interfaces;
using _99phantram.Models;

namespace _99phantram.Controllers.Apps
{
  [Route("/api/app/specs")]
  [ApiController]
  public class SpecController : ControllerBase
  {
    private readonly ISpecService _specService;

    public SpecController(ISpecService specService)
    {
      _specService = specService;
    }

    [HttpPost("{id:length(24)}")]
    [TypeFilter(typeof(AppAuthorize))]
    public async Task<ActionResult> CreateSpec(SpecBody body, string id)
    {
      try
      {
        await _specService.CreateSpec(id, body);

        return StatusCode(201);
      }
      catch (HttpError error)
      {
        return BadRequest(error);
      }
    }

    [HttpPut("{categoryId:length(24)}/{specId:length(24)}")]
    [TypeFilter(typeof(AppAuthorize))]
    public async Task<ActionResult<Spec>> UpdateSpec(SpecBody body, string categoryId, string specId)
    {
      try
      {
        await _specService.UpdateSpec(categoryId, specId, body);

        return StatusCode(204);
      }
      catch (HttpError error)
      {
        return BadRequest(error);
      }
    }

    [HttpDelete("{categoryId:length(24)}/{specId:length(24)}")]
    [TypeFilter(typeof(AppAuthorize))]
    public async Task<ActionResult<Spec>> DeleteSpec(string categoryId, string specId)
    {
      try
      {
        await _specService.DeleteSpec(categoryId, specId);

        return StatusCode(204);
      }
      catch (HttpError error)
      {
        return BadRequest(error);
      }
    }
  }
}