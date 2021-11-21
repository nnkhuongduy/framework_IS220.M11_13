using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _99phantram.Entities;
using _99phantram.Interfaces;
using _99phantram.Models;
using MongoDB.Bson;
using MongoDB.Entities;

namespace _99phantram.Services
{
  public class LocationService : ILocationService
  {
    public LocationService()
    {
      Task.Run(async () =>
      {
        await DB.Index<Location>()
          .Key(user => user.Name, KeyType.Ascending)
          .Option(option => option.Unique = true)
          .CreateAsync();
      }).GetAwaiter().GetResult();
    }


    public async Task<Location> ArchiveLocation(Location location)
    {
      location.Status = LocationStatus.ARCHIVED;

      await location.SaveAsync();
      return location;
    }

    public async Task<Location> CreateLocation(LocationBody body)
    {
      Location location = new Location();

      location.Name = body.Name;
      location.LocationLevel = body.LocationLevel;
      location.Status = body.Status;
      location.SubLocations = new List<Location>();
      location.SubLocationRef = new List<ObjectId>();

      await location.SaveAsync();

      return location;
    }

    public async Task DeleteLocation(string id)
        {
            var deletingLocation = await DB.Find<Location>().MatchID(id).ExecuteFirstAsync();     

            if (deletingLocation.Status == LocationStatus.ARCHIVED)
            {
                await deletingLocation.DeleteAsync();   
            }
            else 
                throw new HttpError(false, 404, "Danh mục không tìm thấy!");
            return;
        }

        public async Task<Location> GetLocation(string id)
    {
      var result = await DB.Find<Location>().Match(_ => _.ID == id).ExecuteFirstAsync();

      if (result == null)
      {
        throw new HttpError(false, 404, "Không tìm thấy danh mục sản phẩm");
      }

      return result;
    }

    public async Task<Location> UpdateLocation(LocationBody body, string id)
    {
      List<ObjectId> subLocations;

      try
      {
        subLocations = await DB.Find<Location, ObjectId>().Match(_ => _.In(_ => _.ID, body.SubLocations)).Project(_ => ObjectId.Parse(_.ID)).ExecuteAsync();
      }
      catch (Exception)
      {
        throw new HttpError(false, 400, "Không tìm thấy danh mục con");
      }  

      var newLocation = await DB.UpdateAndGet<Location>()
        .MatchID(id)
        .Modify(_ => _.Name, body.Name)
        .Modify(_ => _.LocationLevel, body.LocationLevel)
        .Modify(_ => _.Status, body.Status)
        .Modify(_ => _.SubLocationRef, subLocations)
        .ExecuteAsync();

      return newLocation;
    }
  }
}