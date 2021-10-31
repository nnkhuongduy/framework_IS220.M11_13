using System.Threading.Tasks;
using MongoDB.Entities;

using _99phantram.Entities;
using _99phantram.Interfaces;
using _99phantram.Models;
using System.Linq;

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

      var newSpec = await DB.UpdateAndGet<Spec>().MatchID(spec.ID).Modify(_ => _.Name, body.Name).Modify(_ => _.Required, body.Required).ExecuteAsync();
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

      await spec.DeleteAsync();
      await category.SaveAsync();
    }
  }
}