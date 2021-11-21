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
  [Route("/api/app/service-types")]
  [ApiController]
  [ServiceFilter(typeof(AppAuthorize))]
  public class ServiceTypeController : ControllerBase
  {
    private readonly IServiceTypeService _servicetypeService;
    public ServiceTypeController(IServiceTypeService servicetypeService)
    {
      _servicetypeService = servicetypeService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ServiceType>>> GetAllServiceType()
    {
      return await _servicetypeService.GetAllService();
    }

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<ServiceType>> GetServiceType(string id)
    {
      try
      {
        var servicetype = await _servicetypeService.GetServiceType(id);

        return servicetype;
      }
      catch (HttpError error)
      {

        return NotFound(error);
      }
    }

    [HttpPost]
    public async Task<ActionResult> CreateServiceType(ServiceTypeBody body)
    {
      await _servicetypeService.CreateServiceType(body);

      return StatusCode(201);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<ActionResult<ServiceType>> UpdateServiceType(ServiceTypeBody body, string id)
    {
      try
      {
        var newServiceType = await _servicetypeService.UpdateServiceType(body, id);

        if (newServiceType.Status == ServiceTypeStatus.DEACTIVE)
        {
          await _servicetypeService.ArchiveServiceType(newServiceType);
        }

        return StatusCode(204);
      }
      catch (HttpError error)
      {
        return NotFound(error);
      }
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<ActionResult<ServiceType>> DeleteServiceType(string id)
    {
      try
      {
        await _servicetypeService.DeleteServiceType(id);

        return StatusCode(204);
      }
      catch (HttpError error)
      {
        return NotFound(error);
      }
    }
  }
}