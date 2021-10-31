using System;

namespace _99phantram.Models
{
  public class HttpError : Exception
  {
    public bool Success { get; set; }
    public int Code { get; set; }

    public HttpError(bool success, int code, string message) : base(message)
    {
      Success = success;
      Code = code;
    }
  }
}