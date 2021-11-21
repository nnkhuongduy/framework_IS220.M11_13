using System.Collections.Generic;
using System.Threading.Tasks;
using _99phantram.Entities;
using _99phantram.Models;

namespace _99phantram.Interfaces
{
  public interface IServiceTypeService
  {
    Task<List<ServiceType>> GetAllService();
    Task<ServiceType> GetServiceType(string id);
    Task<ServiceType> CreateServiceType(ServiceTypeBody body);
    Task<ServiceType> UpdateServiceType(string id, ServiceTypeBody body);
    Task DeactivateServiceType(ServiceType serviceType);
    Task DeleteServiceType(string id);
  }
}