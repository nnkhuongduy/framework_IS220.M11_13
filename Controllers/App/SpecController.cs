using System.Linq;
using System.Threading.Tasks;
using _99phantram.Entities;
using _99phantram.Helpers;
using _99phantram.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace _99phantram.Controllers.Apps
{
  [Route("/api/app/specs")]
  [ApiController]
  public class SpecController : ControllerBase
  {
    [HttpPost("{id:length(24)}")]
    [TypeFilter(typeof(AppAuthorize))]
    public async Task<ActionResult> CreateSpec(SpecBody body, string id)
    {
      Category category = await DB.Find<Category>().MatchID(id).ExecuteFirstAsync();

      if (category == null)
      {
        return BadRequest(new HttpError(false, 400, "Danh mục sản phẩm không tìm thấy!"));
      }

      Spec spec = new Spec();
      spec.Name = body.Name;
      spec.Value = "";
      spec.Required = body.Required;
      category.Specs.Add(spec);

      await spec.SaveAsync();

      await DB.Update<Category>().MatchID(id).Modify(_ => _.Specs, category.Specs).ExecuteAsync();

      return StatusCode(201);
    }

    [HttpPut("{categoryId:length(24)}/{specId:length(24)}")]
    [TypeFilter(typeof(AppAuthorize))]
    public async Task<ActionResult<Spec>> UpdateSpec(SpecBody body, string categoryId, string specId)
    {
      var category = await DB.Find<Category>().MatchID(categoryId).ExecuteFirstAsync();

      if (category == null)
      {
        return BadRequest(new HttpError(false, 400, "Danh mục sản phẩm không tìm thấy!"));
      }

      var spec = category.Specs.FirstOrDefault(_ => _.ID == specId);

      if (spec == null)
      {
        return BadRequest(new HttpError(false, 400, "Chi tiết danh mục không tìm thấy!"));
      }

      var newSpec = await DB.UpdateAndGet<Spec>().MatchID(spec.ID).Modify(_ => _.Name, body.Name).Modify(_ => _.Required, body.Required).ExecuteAsync();
      var index = category.Specs.FindIndex(_ => _.ID == specId);
      category.Specs[index] = newSpec;

      await DB.Update<Category>().MatchID(categoryId).Modify(_ => _.Specs, category.Specs).ExecuteAsync();

      return StatusCode(204);
    }

    [HttpDelete("{categoryId:length(24)}/{specId:length(24)}")]
    [TypeFilter(typeof(AppAuthorize))]
    public async Task<ActionResult<Spec>> DeleteSpec(string categoryId, string specId)
    {
      var category = await DB.Find<Category>().MatchID(categoryId).ExecuteFirstAsync();

      if (category == null)
      {
        return BadRequest(new HttpError(false, 400, "Danh mục sản phẩm không tìm thấy!"));
      }

      var spec = category.Specs.FirstOrDefault(_ => _.ID == specId);

      if (spec == null)
      {
        return BadRequest(new HttpError(false, 400, "Chi tiết danh mục không tìm thấy!"));
      }

      var index = category.Specs.FindIndex(_ => _.ID == specId);
      category.Specs.RemoveAt(index);

      await spec.DeleteAsync();
      await category.SaveAsync();

      return StatusCode(204);
    }
  }
}