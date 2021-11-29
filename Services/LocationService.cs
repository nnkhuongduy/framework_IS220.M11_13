using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Entities;
using MongoDB.Bson;

using _99phantram.Entities;
using _99phantram.Interfaces;
using _99phantram.Models;

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

    private async Task _RemoveFromParents(Location location)
    {
      var parentLocations = await DB.Find<Location>().Match(_ => _.SubLocationsRef.Contains(ObjectId.Parse(location.ID))).ExecuteAsync();

      foreach (var _ in parentLocations)
      {
        _.SubLocationsRef.Remove(ObjectId.Parse(location.ID));
        await _.SaveAsync();
      }
    }

    public async Task<Location> ArchiveLocation(Location location)
    {
      var supplies = await DB.Find<Supply>().Match(_ => _.ElemMatch(__ => __.Locations, __ => __.ID == location.ID)).ExecuteAsync();

      if (supplies.Count > 0)
        throw new HttpError(false, 400, "Không thể lưu trữ địa điểm vẫn còn sản phẩm đang sử dụng!");

      location.Status = LocationStatus.ARCHIVED;
      location.SubLocations.Clear();

      await _RemoveFromParents(location);

      await location.SaveAsync();

      return location;
    }

    public async Task<Location> CreateLocation(LocationBody body)
    {
      Location location = new Location();

      location.Name = body.Name;
      location.LocationLevel = body.LocationLevel;
      location.Status = body.Status;
      location.SubLocationsRef = new List<ObjectId>();

      await location.SaveAsync();

      return location;
    }

    public async Task DeleteLocation(string id)
    {
      var deletingLocation = await GetLocation(id);

      if (deletingLocation.Status != LocationStatus.ARCHIVED)
        throw new HttpError(false, 400, "Không thể xóa địa điểm chưa được lưu trữ!");

      await deletingLocation.DeleteAsync();
    }

    public async Task<Location> GetLocation(string id)
    {
      var result = await DB.Find<Location>().Match(_ => _.ID == id).ExecuteFirstAsync();

      if (result == null)
      {
        throw new HttpError(false, 404, "Không tìm thấy địa điểm");
      }

      return result;
    }

    public async Task<Location> UpdateLocation(string id, LocationBody body)
    {
      var location = await GetLocation(id);
      List<ObjectId> subLocations;

      var supplies = await DB.Find<Supply>().Match(_ => _.ElemMatch(__ => __.Locations, __ => __.ID == id)).ExecuteAsync();

      if (body.Status == LocationStatus.ARCHIVED)
      {
        location = await ArchiveLocation(location);
      }

      if (location.LocationLevel != body.LocationLevel)
      {
        if (supplies.Count > 0)
          throw new HttpError(false, 400, "Không thể thay đổi cấp địa điểm khi vẫn còn sản phẩm đang sử dụng!");

        await _RemoveFromParents(location);
        subLocations = new List<ObjectId>();
      }
      else
      {
        if (body.SubLocations != null) 
          subLocations = await DB.Find<Location, ObjectId>().Match(_ => _.In(__ => __.ID, body.SubLocations)).Project(_ => ObjectId.Parse(_.ID)).ExecuteAsync();
        else subLocations = location.SubLocationsRef;
      }

      location.Name = body.Name;
      location.LocationLevel = body.LocationLevel;
      location.Status = body.Status;
      location.SubLocationsRef = subLocations;

      await location.SaveAsync();

      foreach (var supply in supplies)
      {
        var supplyLocation = supply.Locations.Find(_ => _.ID == id);

        supplyLocation.Name = body.Name;
        supplyLocation.LocationLevel = body.LocationLevel;
        supplyLocation.Status = body.Status;

        await supply.SaveAsync();
      }

      return location;
    }
  }
}