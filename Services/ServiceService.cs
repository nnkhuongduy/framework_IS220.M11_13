using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _99phantram.Entities;
using _99phantram.Interfaces;
using _99phantram.Models;
using MongoDB.Bson;
using MongoDB.Entities;

namespace _99phantram.Services
{
  public class ServiceService : IServiceService
  {
    public ServiceService()
    {
      Task.Run(async () =>
      {
        await DB.Index<Service>()
          .Key(user => user.Name, KeyType.Ascending)
          .Option(option => option.Unique = true)
          .CreateAsync();
      }).GetAwaiter().GetResult();
    }

    public async Task ExpireService(Service service)
    {
      service.Status = ServiceStatus.EXPIRED;

      await service.SaveAsync();

      return;
    }

    public async Task<Service> CreateService(ServicePostBody body)
    {
      Service service = new Service();

      service.Name = body.Name;
      service.ServiceType = body.ServiceType;
      service.Value = body.Value;
      service.Status = body.Status;

      await service.SaveAsync();

      return service;
    }

    public async Task DeleteCategory(string id)
    {
      var service = await GetService(id);

      if (service.Status != ServiceStatus.EXPIRED)
      {
        throw new HttpError(false, 404, "Dịch vụ không thể xóa!");
      }

      await service.DeleteAsync();
    }

    public async Task<List<Service>> GetAllServices()
    {
      return await DB.Find<Service>().Match(_ => true).Sort(_ => _.Name, MongoDB.Entities.Order.Ascending).ExecuteAsync();
    }

    public async Task<Service> GetService(string id)
    {
      var result = await DB.Find<Service>().Match(_ => _.ID == id).ExecuteFirstAsync();

      if (result == null)
      {
        throw new HttpError(false, 404, "Không tìm thấy dịch vụ");
      }

      return result;
    }

    public async Task<Service> UpdateService(ServicePutBody body, string id)
    {
      var newService = await DB.UpdateAndGet<Service>()
        .MatchID(id)
        .Modify(_ => _.Name, body.Name)
        .Modify(_ => _.Value, body.Value)
        .Modify(_ => _.Status, body.Status)
        .ExecuteAsync();

      return newService;
    }
  }
} 
