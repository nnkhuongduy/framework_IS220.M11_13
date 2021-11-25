using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using _99phantram.Entities;
using _99phantram.Helpers;
using _99phantram.Interfaces;
using _99phantram.Models;

namespace _99phantram.Controllers.Apps
{
  [Route("/api/app/services")]
  [ApiController]
  [ServiceFilter(typeof(AppAuthorize))]
  public class ServiceController : ControllerBase
  {
    private readonly IServiceService _serviceService;
    public ServiceController(IServiceService serviceService)
    {
      _serviceService = serviceService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Service>>> GetAllServices()
    {
      return await _serviceService.GetAllServices();
    }

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Service>> GetService(string id)
    {
      try
      {
        var service = await _serviceService.GetService(id);

        return service;
      }
      catch (HttpError error)
      {
        return StatusCode(error.Code, error);
      }
    }

    [HttpPost]
    public async Task<ActionResult> CreateService(ServicePostBody body)
    {
      await _serviceService.CreateService(body);

      return StatusCode(201);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<ActionResult<Service>> UpdateService(ServicePutBody body, string id)
    {
      try
      {
        var newService = await _serviceService.UpdateService(body, id);

        return StatusCode(204);
      }
      catch (HttpError error)
      {
        return StatusCode(error.Code, error);
      }
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<ActionResult<Service>> DeleteService(string id)
    {
      try
      {
        await _serviceService.DeleteService(id);

        return StatusCode(204);
      }
      catch (HttpError error)
      {
        return StatusCode(error.Code, error);
      }
    }
  }
}
