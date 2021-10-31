using System.Threading.Tasks;

using _99phantram.Entities;
using _99phantram.Models;

namespace _99phantram.Interfaces
{
  public interface ISpecService
  {
    Task<Spec> CreateSpec(string categoryId, SpecBody body);
    Task<Spec> UpdateSpec(string categoryId, string specId, SpecBody body);
    Task DeleteSpec(string categoryId, string specId);
  }
}