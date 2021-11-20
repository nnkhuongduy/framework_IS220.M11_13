using System.Collections.Generic;
using System.Threading.Tasks;
using _99phantram.Entities;
using _99phantram.Models;

namespace _99phantram.Interfaces
{
  public interface IServiceService
  {
    Task<List<Service>> GetAllServices();
    Task<Service> GetService(string id);
    Task<Service> CreateService(ServicePostBody body);
    Task<Service> UpdateService(ServicePutBody body, string id);
    Task ArchiveService(Service service);
    Task DeleteService(string id);
  }
} 
