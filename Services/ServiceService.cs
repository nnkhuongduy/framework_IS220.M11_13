using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Entities;
using Newtonsoft.Json;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;

using _99phantram.Entities;
using _99phantram.Interfaces;
using _99phantram.Models;

namespace _99phantram.Services
{
  public class ServiceService : IServiceService
  {
    private readonly IServiceTypeService _serviceTypeService;

    public ServiceService(IServiceTypeService serviceTypeService)
    {
      Task.Run(async () =>
      {
        await DB.Index<Service>()
          .Key(user => user.Name, KeyType.Ascending)
          .Option(option => option.Unique = true)
          .CreateAsync();
      }).GetAwaiter().GetResult();

      _serviceTypeService = serviceTypeService;
    }

    public async Task<Service> ExpireService(Service service)
    {
      service.Status = ServiceStatus.EXPIRED;

      await service.SaveAsync();

      return service;
    }

    public async Task<Service> CreateService(ServicePostBody body)
    {
      var serviceType = await DB.Find<ServiceType>().MatchID(body.ServiceType).ExecuteFirstAsync();

      if (serviceType == null)
      {
        throw new HttpError(false, 404, "Không tìm thấy kiểu dịch vụ!");
      }

      Service service = new Service();

      var jsonDoc = JsonConvert.SerializeObject(body.Value);

      service.ServiceTypeRef = serviceType.ToReference();
      service.Name = body.Name;
      service.ValueBson = BsonSerializer.Deserialize<BsonDocument>(jsonDoc);
      service.Status = body.Status;

      await service.SaveAsync();

      return service;
    }

    public async Task DeleteService(string id)
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
      var services = await DB.Find<Service>().Match(_ => true).Sort(_ => _.CreatedOn, MongoDB.Entities.Order.Descending).ExecuteAsync();

      foreach (var service in services)
      {
        var bsonDoc = BsonExtensionMethods.ToJson(service.ValueBson);
        service.Value = JsonConvert.DeserializeObject<Dictionary<string, object>>(bsonDoc);
        service.ServiceType = await _serviceTypeService.GetServiceType(service.ServiceTypeRef.ID);
      }

      return services;
    }

    public async Task<Service> GetService(string id)
    {
      var service = await DB.Find<Service>().MatchID(id).ExecuteFirstAsync();

      if (service == null)
      {
        throw new HttpError(false, 404, "Không tìm thấy dịch vụ!");
      }

      var bsonDoc = BsonExtensionMethods.ToJson(service.ValueBson);
      service.Value = JsonConvert.DeserializeObject<Dictionary<string, object>>(bsonDoc);
      service.ServiceType = await _serviceTypeService.GetServiceType(service.ServiceTypeRef.ID);

      return service;
    }

    public async Task<Service> UpdateService(ServicePutBody body, string id)
    {
      var service = await GetService(id);

      var jsonDoc = JsonConvert.SerializeObject(body.Value);

      service.ValueBson = BsonSerializer.Deserialize<BsonDocument>(jsonDoc);
      service.Name = body.Name;
      service.Status = body.Status;

      if (body.Status == ServiceStatus.EXPIRED)
        service = await ExpireService(service);

      await service.SaveAsync();

      return service;
    }
  }
}
