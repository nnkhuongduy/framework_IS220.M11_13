using System.Collections.Generic;
using System.Threading.Tasks;
using _99phantram.Entities;
using _99phantram.Helpers;
using _99phantram.Interfaces;
using _99phantram.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace _99phantram.Controllers.Apps
{
  [Route("/api/app/categories")]
  [ApiController]
  public class CategoryController : ControllerBase
  {
    private readonly ICategoryService _categoryService;
    public CategoryController(ICategoryService categoryService)
    {
      _categoryService = categoryService;
    }

    [HttpGet]
    [TypeFilter(typeof(AppAuthorize))]
    public async Task<ActionResult<List<Category>>> GetAllCategories()
    {
      return await _categoryService.GetAllCategories();
    }

    [HttpGet("{id:length(24)}")]
    [TypeFilter(typeof(AppAuthorize))]
    public async Task<ActionResult<Category>> GetCategory(string id)
    {
      try
      {
        var category = await _categoryService.GetCategory(id);

        return category;
      }
      catch (HttpError error)
      {

        return NotFound(error);
      }
    }

    [HttpPost]
    [TypeFilter(typeof(AppAuthorize))]
    public async Task<ActionResult> CreateCategory(PostCategoryBody body)
    {
      await _categoryService.CreateCategory(body);

      return StatusCode(201);
    }

    [HttpPut("{id:length(24)}")]
    [TypeFilter(typeof(AppAuthorize))]
    public async Task<ActionResult<Category>> UpdateCategory(PutCategoryBody body, string id)
    {
      try
      {
        var newCategory = await _categoryService.UpdateCategory(body, id);

        if (newCategory.Status == CategoryStatus.ARCHIVED)
        {
          await _categoryService.ArchiveCategory(newCategory);
        }

        return StatusCode(204);
      }
      catch (HttpError error)
      {
        return NotFound(error);
      }
    }

    [HttpDelete("{id:length(24)}")]
    [TypeFilter(typeof(AppAuthorize))]
    public async Task<ActionResult<Category>> DeleteCategory(string id)
    {
      try
      {
        await _categoryService.DeleteCategory(id);

        return StatusCode(204);
      }
      catch (HttpError error)
      {
        return NotFound(error);
      }
    }
  }
}