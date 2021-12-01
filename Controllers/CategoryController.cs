using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using MongoDB.Entities;
using MongoDB.Bson;

using _99phantram.Interfaces;
using _99phantram.Entities;

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

    [HttpGet("secondary/{id:length(24)}")]
    public async Task<ActionResult<List<Category>>> GetSecondaryCategories(string id)
    {
      var refs = await DB.Find<Category, List<ObjectId>>().MatchID(id).Project(_ => _.SubCategories).ExecuteFirstAsync();

      return await DB.Find<Category>().Match(_ => _.In("_id", refs)).Project(_ => _.Exclude("sub_categories")).ExecuteAsync();
    }
  }
}