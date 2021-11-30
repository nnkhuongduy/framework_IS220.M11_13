using Microsoft.Extensions.Configuration;

using _99phantram.Interfaces;
using System.Net.Mail;
using System.Net;
using _99phantram.Entities;
using System.IO;

namespace _99phantram.Services
{
  public class MailService : IMailService
  {
    private readonly string _serverName;
    private readonly int _port;
    private readonly string _username;
    private readonly string _password;
    private readonly string _hostName;

    public MailService(IConfiguration configuration)
    {
      var mailSettings = configuration.GetSection("Mail");

      this._serverName = mailSettings.GetValue<string>("ServerName");
      this._port = mailSettings.GetValue<int>("Port");
      this._username = mailSettings.GetValue<string>("Username");
      this._password = mailSettings.GetValue<string>("Password");

      this._hostName = configuration.GetValue<string>("HostName");
    }

    private void _SendEmail(MailMessage mailMessage)
    {
      using var client = new SmtpClient(this._serverName, this._port)
      {
        Credentials = new NetworkCredential(this._username, this._password),
        EnableSsl = true,

      };

      client.Send(mailMessage);
    }

    public void SendRegistrationVerification(User user)
    {
      var str = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "Emails", "Verification.html"));
      string body = str.ReadToEnd();
      str.Close();

      body = body.Replace("{{ verification_link }}", $"{_hostName}/verification/{user.ID}");

      var mailMessage = new MailMessage();

      mailMessage.From = new MailAddress("noreply@99phantram.com");
      mailMessage.To.Add(user.Email);
      mailMessage.Subject = "Xác nhận tài khoản 99phantram";
      mailMessage.Body = body;
      mailMessage.IsBodyHtml = true;

      _SendEmail(mailMessage);
    }
  }
}