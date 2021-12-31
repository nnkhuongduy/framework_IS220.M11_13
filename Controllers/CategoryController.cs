using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using MongoDB.Entities;
using MongoDB.Bson;

using _99phantram.Interfaces;
using _99phantram.Entities;
using _99phantram.Models;

namespace _99phantram.Controllers
{
  [Route("/api/category")]
  [ApiController]
  public class ClientCategoryController : ControllerBase
  {
    private readonly ICategoryService _categoryService;
    public ClientCategoryController(ICategoryService categoryService)
    {
      _categoryService = categoryService;
    }

    [HttpGet("primary")]
    public async Task<ActionResult<List<Category>>> GetAllPrimaryCategories()
    {
      return await DB
        .Find<Category>()
        .Match(_ => _.Status != CategoryStatus.ARCHIVED && _.CategoryLevel == CategoryLevel.PRIMARY)
        .Project(_ => _.Exclude("sub_categories"))
        .ExecuteAsync();
    }

    [HttpGet("secondary/{idOrSlug}")]
    public async Task<ActionResult<List<Category>>> GetSecondaryCategories(string idOrSlug)
    {
      var refs = await DB.Find<Category, List<ObjectId>>().Match(_ => _.ID == idOrSlug || _.Slug == idOrSlug).Project(_ => _.SubCategories).ExecuteFirstAsync();

      return await DB.Find<Category>().Match(_ => _.In("_id", refs)).Project(_ => _.Exclude("sub_categories")).ExecuteAsync();
    }

    [HttpGet("detail/{idOrSlug}")]
    public async Task<ActionResult<Category>> GetCategory(string idOrSlug)
    {
      var category = await DB.Find<Category>().Match(_ => _.ID == idOrSlug || _.Slug == idOrSlug).ExecuteFirstAsync();

      if (category == null)
        return StatusCode(404, new HttpError(false, 404, "Không tìm thấy danh mục!"));

      return category;
    }
  }
}