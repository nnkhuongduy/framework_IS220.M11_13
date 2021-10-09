namespace _99phantram.Models
{
  public class HttpError
  {
    public bool Success { get; set; }
    public int Code { get; set; }
    public string Message { get; set; }

    public HttpError(bool success, int code, string message)
    {
      Success = success;
      Code = code;
      Message = message;
    }
  }
}