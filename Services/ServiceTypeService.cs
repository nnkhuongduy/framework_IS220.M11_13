using System.Collections.Generic;
using System.Threading.Tasks;

using _99phantram.Entities;
using _99phantram.Interfaces;
using _99phantram.Models;
using MongoDB.Entities;

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
     public async Task<List<ServiceType>> GetAllService()
    {
      return await DB.Find<ServiceType>().Match(_ => true).Sort(_ => _.Name, MongoDB.Entities.Order.Ascending).ExecuteAsync();
    }


    //Get service type
    public async Task<ServiceType> GetServiceType(string id)
    {
      var result = await DB.Find<ServiceType>().Match(_ => _.ID == id).ExecuteFirstAsync();

      if (result == null)
      {
        throw new HttpError(false, 404, "Không tìm thấy loại dịch vụ nào!");
      }
      return result;
    }

    //Create service type
    public async Task<ServiceType> CreateServiceType(ServiceTypeBody body)
    {
      ServiceType servicetype = new ServiceType();

      servicetype.Name = body.Name;
      servicetype.Status = body.Status;
      servicetype.Value = new Dictionary<string, object>();
      servicetype.ValueBson = body.Value;

      await servicetype.SaveAsync();

      return servicetype;
    }

    //Update service
     public async Task<ServiceType> UpdateServiceType(string id, ServiceTypeBody body)
     
    {
      var updateServiceType = await DB.Find<ServiceType>().MatchID(id).ExecuteFirstAsync();

      if (updateServiceType == null)
      {
        throw new HttpError(false, 404, "Không tìm thấy dịch vụ!");
      }
      var newServiceType = await DB.UpdateAndGet<ServiceType>()
        .MatchID(id)
        .Modify(_ => _.Name, body.Name)
        .Modify(_ => _.Status, body.Status)
        .Modify(_ => _.Value, new Dictionary<string, object>())
        .Modify(_ => _.ValueBson, body.Value)
        .ExecuteAsync();

      return newServiceType;
    }

    //Deactive Service
      public async Task DeactivateServiceType(ServiceType serviceType)
    {
      serviceType.Status = ServiceTypeStatus.DEACTIVE;

      await serviceType.SaveAsync();

      return;
    }

    //Delete Service
     public async Task DeleteServiceType(string id)
      {
            var deletingServiceType = await DB.Find<ServiceType>().MatchID(id).ExecuteFirstAsync();     

            if (deletingServiceType.Status == ServiceTypeStatus.DEACTIVE)
            {
                await deletingServiceType.DeleteAsync();   
            }
            else 
                throw new HttpError(false, 404, "không tìm thấy dịch vụ!");
            return;
        }
  }
}