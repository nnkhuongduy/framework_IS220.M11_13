using System.Threading.Tasks;
using _99phantram.Entities;

namespace _99phantram.Interfaces
{
  public interface IMailService
  {
    void SendRegistrationVerification(User user);
    Task SendSupplySubmitted(Supply supply);
    Task SendSupplyToActive(Supply supply);
    Task SendSupplyToDeclined(Supply supply);
    Task SendSupplyToArchive(Supply supply);
  }
}