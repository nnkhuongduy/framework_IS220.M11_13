using System.Threading.Tasks;
using System.Collections.Generic;
using MongoDB.Entities;

using _99phantram.Entities;
using _99phantram.Interfaces;
using _99phantram.Models;

namespace _99phantram.Services
{
  public class SupplyService : ISupplyService
  {
    private readonly IMailService _mailService;

    public SupplyService(IMailService mailService)
    {
      _mailService = mailService;
    }

    public async Task<Supply> CreateSupply(User owner, ClientPostSupply body)
    {
      var specs = new List<Spec>();
      var categories = new List<Category>();
      var locations = new List<Location>();

      foreach (var specBody in body.Specs)
      {
        var spec = await DB.Find<Spec>().MatchID(specBody.Id).ExecuteFirstAsync();

        if (spec == null)
          throw new HttpError(false, 400, "Không tìm thấy thiết lập!");

        var newSpec = new Spec();
        newSpec.Name = spec.Name;
        newSpec.Value = specBody.Value;
        newSpec.Required = spec.Required;
        newSpec.Parent = spec.Parent;

        specs.Add(newSpec);
      }

      foreach (var categoriesId in body.Categories)
      {
        var category = await DB.Find<Category>().MatchID(categoriesId).ProjectExcluding(_ => new { _.SubCategories, _.Specs }).ExecuteFirstAsync();

        if (category == null)
          throw new HttpError(false, 400, "Không tìm thấy danh mục!");

        categories.Add(category);
      }

      foreach (var locationId in body.Locations)
      {
        var location = await DB.Find<Location>().MatchID(locationId).Project(_ => _.Exclude("sub_locations")).ExecuteFirstAsync();

        if (location == null)
          throw new HttpError(false, 400, "Không tìm thấy địa điểm!");

        locations.Add(location);
      }

      var supply = new Supply();

      supply.OwnerRef = owner;
      supply.Name = body.Name;
      supply.Price = body.Price;
      supply.Description = body.Description;
      supply.Services = new List<Service>();
      supply.Specs = specs;
      supply.Images = body.Images;
      supply.Thumbnail = body.Thumbnail;
      supply.Categories = categories;
      supply.Locations = locations;
      supply.Address = body.Address;
      supply.Reason = "";
      supply.ProductStatus = ProductStatus._99;
      supply.Status = SupplyStatus.WAITING;

      await supply.SaveAsync();

      await _mailService.SendSupplySubmitted(supply);

      return supply;
    }

    public async Task<List<Supply>> GetAllSupplies()
    {
      var supplies = await DB
        .Find<Supply>()
        .Match(_ => true)
        .Sort(_ => _.CreatedOn, MongoDB.Entities.Order.Descending)
        .ExecuteAsync();

      foreach (var supply in supplies)
        supply.Owner = await supply.OwnerRef.ToEntityAsync();

      return supplies;
    }

    public async Task<List<Supply>> GetActiveSupplies(SupplyQueryFilter filter)
    {
      var supplies = await DB
        .Find<Supply>()
        .Match(_ => _.Status == SupplyStatus.ACTIVE)
        .Sort(_ => _.CreatedOn, MongoDB.Entities.Order.Descending)
        .Limit(20)
        .Skip(20 * filter.Page)
        .ExecuteAsync();

      foreach (var supply in supplies)
        supply.Owner = await supply.OwnerRef.ToEntityAsync();

      return supplies;
    }

    public async Task<Supply> GetSupply(string id)
    {
      var supply = await DB.Find<Supply>().MatchID(id).ExecuteFirstAsync();

      if (supply == null)
        throw new HttpError(false, 404, "Không tìm thấy sản phẩm!");

      supply.Owner = await supply.OwnerRef.ToEntityAsync();

      return supply;
    }

    public async Task<Supply> UpdateSupply(string id, PutSupply body)
    {
      var supply = await GetSupply(id);

      supply.Status = body.Status;

      if (!string.IsNullOrEmpty(body.Reason))
      {
        supply.Reason = body.Reason;
      }

      await supply.SaveAsync();

      if (body.SendEmail)
      {
        if (body.Status == SupplyStatus.DECLINED)
          await _mailService.SendSupplyToDeclined(supply);
        if (body.Status == SupplyStatus.ACTIVE)
          await _mailService.SendSupplyToActive(supply);
        if (body.Status == SupplyStatus.ARCHIVED)
          await _mailService.SendSupplyToArchive(supply);
      }


      return supply;
    }

    public async Task DeleteSupply(string id)
    {
      var supply = await GetSupply(id);

      if (supply.Status != SupplyStatus.ARCHIVED && supply.Status != SupplyStatus.DECLINED)
        throw new HttpError(false, 400, "Không thể xóa sản phẩm chưa được lưu trữ hoặc từ chối!");

      await supply.DeleteAsync();
    }

    public async Task<List<Supply>> GetOwnSupplies(User user)
    {
      return await DB
        .Find<Supply>()
        .Match(_ => _.OwnerRef.ID == user.ID && _.Status != SupplyStatus.ARCHIVED)
        .Sort(_ => _.CreatedOn, MongoDB.Entities.Order.Descending)
        .ExecuteAsync();
    }
  }
}
