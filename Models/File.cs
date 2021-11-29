using System.Collections.Generic;

namespace _99phantram.Models
{
  public class GetFileResponse
  {
    public string Url { get; set; }
  }

  public class GetFilesResponse
  {
    public List<string> Urls { get; set; }

    public GetFilesResponse()
    {
      Urls = new List<string>();
    }
  }
}