using System.Threading.Tasks;
using MongoDB.Entities;
using System.Linq;
using MongoDB.Bson;

using _99phantram.Entities;
using _99phantram.Interfaces;
using _99phantram.Models;

namespace _99phantram.Services
{
  public class SpecService : ISpecService
  {
    private readonly ICategoryService _categoryService;

    public SpecService(ICategoryService categoryService)
    {
      _categoryService = categoryService;
    }

    public async Task<Spec> CreateSpec(string categoryId, SpecBody body)
    {
      Category category = await _categoryService.GetCategory(categoryId);

      Spec spec = new Spec();
      spec.Name = body.Name;
      spec.Value = "";
      spec.Required = body.Required;
      category.Specs.Add(spec);

      await spec.SaveAsync();

      await category.SaveAsync();

      var supplies = await DB.Find<Supply>().Match(_ => _.ElemMatch(__ => __.Categories, __ => __.ID.Equals(ObjectId.Parse(categoryId)))).ExecuteAsync();

      foreach (var supply in supplies)
      {
        Spec supplySpec = new Spec();
        supplySpec.Name = body.Name;
        supplySpec.Value = "";
        supplySpec.Required = body.Required;
        supplySpec.Parent = ObjectId.Parse(spec.ID);

        supply.Specs.Add(supplySpec);

        await supply.SaveAsync();
      }

      return spec;
    }

    public async Task<Spec> UpdateSpec(string categoryId, string specId, SpecBody body)
    {
      var category = await _categoryService.GetCategory(categoryId);

      var spec = category.Specs.FirstOrDefault(_ => _.ID == specId);

      if (spec == null)
      {
        throw new HttpError(false, 400, "Chi tiết danh mục không tìm thấy!");
      }

      var supplies = await DB.Find<Supply>().Match(_ => _.ElemMatch(__ => __.Specs, __ => __.Parent.Equals(ObjectId.Parse(specId)))).ExecuteAsync();

      var newSpec = await DB.UpdateAndGet<Spec>().MatchID(spec.ID).Modify(_ => _.Name, body.Name).Modify(_ => _.Required, body.Required).ExecuteAsync();

      foreach (var supply in supplies)
      {
        var supplySpec = supply.Specs.Find(_ => _.Parent.Equals(ObjectId.Parse(specId)));

        supplySpec.Name = body.Name;
        supplySpec.Required = body.Required;

        await supply.SaveAsync();
      }

      var index = category.Specs.FindIndex(_ => _.ID == specId);

      category.Specs[index] = newSpec;

      await category.SaveAsync();

      return newSpec;
    }

    public async Task DeleteSpec(string categoryId, string specId)
    {
      var category = await _categoryService.GetCategory(categoryId);

      var spec = category.Specs.FirstOrDefault(_ => _.ID == specId);

      if (spec == null)
      {
        throw new HttpError(false, 400, "Chi tiết danh mục không tìm thấy!");
      }

      var index = category.Specs.FindIndex(_ => _.ID == specId);
      category.Specs.RemoveAt(index);

      var supplies = await DB.Find<Supply>().Match(_ => _.ElemMatch(__ => __.Specs, __ => __.Parent.Equals(ObjectId.Parse(specId)))).ExecuteAsync();

      foreach (var supply in supplies)
      {
        var specIndex = supply.Specs.FindIndex(_ => _.Parent.Equals(ObjectId.Parse(specId)));

        supply.Specs.RemoveAt(specIndex);

        await supply.SaveAsync();
      }

      await spec.DeleteAsync();
      await category.SaveAsync();
    }
  }
}