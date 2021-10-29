using System.IO;
using System.Threading.Tasks;

namespace _99phantram.Interfaces
{
  public interface IAmazonS3Context
  {
    Task UploadFile(string bucketName, string keyName, Stream inputStream);
    string GetFileUrl(string bucketName, string keyName);
  }
}