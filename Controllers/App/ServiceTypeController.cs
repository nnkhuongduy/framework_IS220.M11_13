using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using _99phantram.Entities;
using _99phantram.Helpers;
using _99phantram.Interfaces;
using _99phantram.Models;

namespace _99phantram.Controllers
{
  [Route("/api/app/service-types")]
  [ApiController]
  [ServiceFilter(typeof(AppAuthorize))]
  public class ServiceTypeController : ControllerBase
  {
    private readonly IServiceTypeService _serviceTypeService;
    public ServiceTypeController(IServiceTypeService servicetypeService)
    {
      _serviceTypeService = servicetypeService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ServiceType>>> GetAllServiceTypes()
    {
      return await _serviceTypeService.GetAllServiceTypes();
    }

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<ServiceType>> GetServiceType(string id)
    {
      try
      {
        return await _serviceTypeService.GetServiceType(id);
      }
      catch (HttpError error)
      {
        return StatusCode(error.Code, error);
      }
    }

    [HttpPost]
    public async Task<ActionResult> CreateServiceType(ServiceTypeBody body)
    {
      await _serviceTypeService.CreateServiceType(body);

      return StatusCode(201);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<ActionResult> UpdateServiceType(string id, ServiceTypeBody body)
    {
      try
      {
        var serviceType = await _serviceTypeService.UpdateServiceType(id, body);

        if (body.Status == ServiceTypeStatus.DEACTIVE)
        {
          await _serviceTypeService.DeactivateServiceType(serviceType);
        }

        return StatusCode(204);
      }
      catch (HttpError error)
      {
        return StatusCode(error.Code, error);
      }
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<ActionResult> DeleteServiceType(string id)
    {
      try
      {
        await _serviceTypeService.DeleteServiceType(id);

        return StatusCode(204);
      }
      catch (HttpError error)
      {
        return StatusCode(error.Code, error);
      }
    }
  }
}