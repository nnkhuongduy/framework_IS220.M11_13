using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using _99phantram.Interfaces;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace _99phantram.Services
{
  public class AmazonS3Context : IAmazonS3Context
  {
    private readonly string _region;
    private IAmazonS3 client;

    public AmazonS3Context(IConfiguration config)
    {
      _region = config["Aws:Region"];

      client = new AmazonS3Client(config["Aws:AccessKeyID"], config["Aws:SecretAccessKey"], RegionEndpoint.APSoutheast1);
    }
    public async Task UploadFile(string bucketName, string keyName, Stream inputStream)
    {
      var response = await client.PutObjectAsync(new PutObjectRequest
      {
        BucketName = bucketName,
        Key = keyName,
        InputStream = inputStream
      });
    }

    public string GetFileUrl(string bucketName, string keyName)
    {
      return $"https://{bucketName}.s3.{_region}.amazonaws.com/{keyName}";
    }
  }
}