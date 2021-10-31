using _99phantram.Interfaces;
using Microsoft.Extensions.Configuration;

namespace _99phantram.Options
{
  public class AmazonS3Options : IAmazonS3Options
  {
    public string AvatarsBucket { get; set; }
    public string ImagesBucket { get; set; }

    public AmazonS3Options(IConfiguration config)
    {
      var appSettings = config.GetSection(nameof(AmazonS3Options));

      AvatarsBucket = appSettings["AvatarsBucket"];
      ImagesBucket = appSettings["ImagesBucket"];
    }
  }
}