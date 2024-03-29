using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using _99phantram.Interfaces;
using _99phantram.Models;

namespace _99phantram.Controllers
{
  [Route("/api/file")]
  [ApiController]
  public class FileController : ControllerBase
  {
    private readonly string[] _permittedExtensions = { ".png", ".jpg" };
    private readonly IAmazonS3Context _amazonS3;
    private readonly IAmazonS3Options _amazonS3Options;

    public FileController(IAmazonS3Context amazonS3, IAmazonS3Options amazonS3Options)
    {
      _amazonS3 = amazonS3;
      _amazonS3Options = amazonS3Options;
    }

    [HttpPost("avatar")]
    public async Task<ActionResult<GetFileResponse>> UploadAvatar(IFormFile avatar)
    {
      var ext = Path.GetExtension(avatar.FileName).ToLowerInvariant();

      if (string.IsNullOrEmpty(ext) || !_permittedExtensions.Contains(ext))
        return BadRequest(new HttpError(false, 400, "File extension not valid!"));

      if (avatar.Length >= 2097152)
        return BadRequest(new HttpError(false, 400, "File size limit is 2MB"));

      var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
      var filename = $"{timestamp}-{Path.GetRandomFileName()}{ext}";

      try
      {
        await _amazonS3.UploadFile(_amazonS3Options.AvatarsBucket, filename, avatar.OpenReadStream());

        var url = _amazonS3.GetFileUrl(_amazonS3Options.AvatarsBucket, filename);

        return new GetFileResponse
        {
          Url = url
        };
      }
      catch (Exception exception)
      {
        Console.WriteLine(exception.Message);

        return StatusCode(500, new HttpError(false, 500, "Error in the uploading process!"));
      }
    }

    [HttpPost("image")]
    public async Task<ActionResult<GetFileResponse>> UploadImage(IFormFile image)
    {
      var ext = Path.GetExtension(image.FileName).ToLowerInvariant();

      if (string.IsNullOrEmpty(ext) || !_permittedExtensions.Contains(ext))
        return BadRequest(new HttpError(false, 400, "File extension not valid!"));

      if (image.Length >= 2097152)
        return BadRequest(new HttpError(false, 400, "File size limit is 2MB"));

      var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
      var filename = $"{timestamp}-{Path.GetRandomFileName()}{ext}";

      try
      {
        await _amazonS3.UploadFile(_amazonS3Options.ImagesBucket, filename, image.OpenReadStream());

        var url = _amazonS3.GetFileUrl(_amazonS3Options.ImagesBucket, filename);

        return new GetFileResponse
        {
          Url = url
        };
      }
      catch (Exception exception)
      {
        Console.WriteLine(exception.Message);

        return StatusCode(500, new HttpError(false, 500, "Error in the uploading process!"));
      }
    }

    [HttpPost("images")]
    public async Task<ActionResult<GetFilesResponse>> UploadImages(List<IFormFile> images)
    {
      var result = new GetFilesResponse();

      foreach (var image in images)
      {
        var ext = Path.GetExtension(image.FileName).ToLowerInvariant();

        if (string.IsNullOrEmpty(ext) || !_permittedExtensions.Contains(ext))
          return BadRequest(new HttpError(false, 400, "File extension not valid!"));

        if (image.Length >= 2097152)
          return BadRequest(new HttpError(false, 400, "File size limit is 2MB"));

        var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        var filename = $"{timestamp}-{Path.GetRandomFileName()}{ext}";

        try
        {
          await _amazonS3.UploadFile(_amazonS3Options.ImagesBucket, filename, image.OpenReadStream());

          var url = _amazonS3.GetFileUrl(_amazonS3Options.ImagesBucket, filename);

          result.Urls.Add(url);
        }
        catch (Exception exception)
        {
          Console.WriteLine(exception.Message);

          return StatusCode(500, new HttpError(false, 500, "Error in the uploading process!"));
        }
      }

      return result;
    }

    [HttpPost("thumbnail")]
    public async Task<ActionResult<GetFileResponse>> UploadThumbnail(IFormFile thumbnail)
    {
      var ext = Path.GetExtension(thumbnail.FileName).ToLowerInvariant();

      if (string.IsNullOrEmpty(ext) || !_permittedExtensions.Contains(ext))
        return BadRequest(new HttpError(false, 400, "File extension not valid!"));

      if (thumbnail.Length >= 8000000)
        return BadRequest(new HttpError(false, 400, "File size limit is 8MB"));

      var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
      var filename = $"{timestamp}-{Path.GetRandomFileName()}{ext}";

      try
      {
        await _amazonS3.UploadFile(_amazonS3Options.ImagesBucket, filename, thumbnail.OpenReadStream());

        var url = _amazonS3.GetFileUrl(_amazonS3Options.ImagesBucket, filename);

        return new GetFileResponse
        {
          Url = url
        };
      }
      catch (Exception exception)
      {
        Console.WriteLine(exception.Message);

        return StatusCode(500, new HttpError(false, 500, "Error in the uploading process!"));
      }
    }
  }
}