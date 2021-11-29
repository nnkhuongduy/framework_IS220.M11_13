using _99phantram.Entities;

namespace _99phantram.Interfaces
{
  public interface IMailService
  {
    void SendRegistrationVerification(User user);
  }
}