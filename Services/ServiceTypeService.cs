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
  public class ServiceTypeService : IServiceTypeService
  {
    public ServiceTypeService()
    {
      Task.Run(async () =>
      {
        await DB.Index<Category>()
          .Key(user => user.Name, KeyType.Ascending)
          .Option(option => option.Unique = true)
          .CreateAsync();
      }).GetAwaiter().GetResult();
    }

    // Get all service type
    public async Task<List<ServiceType>> GetAllServiceTypes()
    {
      var serviceTypes = await DB
        .Find<ServiceType>()
        .Match(_ => true)
        .Sort(_ => _.ID, MongoDB.Entities.Order.Descending)
        .ExecuteAsync();

      foreach (var serviceType in serviceTypes)
      {
        var bsonDoc = BsonExtensionMethods.ToJson(serviceType.ValueBson);
        serviceType.Value = JsonConvert.DeserializeObject<Dictionary<string, object>>(bsonDoc);
      }

      return serviceTypes;
    }

    //Get service type
    public async Task<ServiceType> GetServiceType(string id)
    {
      var serviceType = await DB.Find<ServiceType>().MatchID(id).ExecuteFirstAsync();

      if (serviceType == null)
      {
        throw new HttpError(false, 404, "Không tìm thấy kiểu dịch vụ!");
      }

      var bsonDoc = BsonExtensionMethods.ToJson(serviceType.ValueBson);
      serviceType.Value = JsonConvert.DeserializeObject<Dictionary<string, object>>(bsonDoc);

      return serviceType;
    }

    //Create service type
    public async Task<ServiceType> CreateServiceType(ServiceTypeBody body)
    {
      ServiceType serviceType = new ServiceType();

      var jsonDoc = JsonConvert.SerializeObject(body.Value);

      serviceType.Name = body.Name;
      serviceType.Status = body.Status;
      serviceType.ValueBson = BsonSerializer.Deserialize<BsonDocument>(jsonDoc);

      await serviceType.SaveAsync();

      return serviceType;
    }

    //Update service
    public async Task<ServiceType> UpdateServiceType(string id, ServiceTypeBody body)
    {
      var serviceType = await GetServiceType(id);

      var jsonDoc = JsonConvert.SerializeObject(body.Value);

      serviceType.Name = body.Name;
      serviceType.Status = body.Status;
      serviceType.ValueBson = BsonSerializer.Deserialize<BsonDocument>(jsonDoc);

      await serviceType.SaveAsync();

      return serviceType;
    }

    //Deactive Service
    public async Task DeactivateServiceType(ServiceType serviceType)
    {
      var services = await DB.Find<Service>().Match(_ => _.ServiceTypeRef.ID == serviceType.ID && _.Status == ServiceStatus.ACTIVE).ExecuteAsync();

      if (services.Count > 0)
      {
        throw new HttpError(false, 400, "Vẫn còn dịch vụ hoạt động sử dụng loại dịch vụ này!");
      }

      serviceType.Status = ServiceTypeStatus.DEACTIVE;

      await serviceType.SaveAsync();
    }

    //Delete Service
    public async Task DeleteServiceType(string id)
    {
      var serviceType = await GetServiceType(id);

      if (serviceType.Status != ServiceTypeStatus.DEACTIVE)
      {
        throw new HttpError(false, 400, "Không thể xóa dịch vụ chưa được lưu trữ!");
      }

      await DB.DeleteAsync<Service>(_ => _.ServiceTypeRef.ID == id);

      await serviceType.DeleteAsync();
    }
  }
}