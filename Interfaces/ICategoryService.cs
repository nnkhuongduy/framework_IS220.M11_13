using System.Collections.Generic;
using System.Threading.Tasks;
using _99phantram.Entities;
using _99phantram.Models;

namespace _99phantram.Interfaces
{
  public interface ICategoryService
  {
    Task<List<Category>> GetAllCategories();
    Task<Category> GetCategory(string id);
    Task<Category> CreateCategory(PostCategoryBody body);
    Task<Category> UpdateCategory(PutCategoryBody body, string id);
    Task<Category> ArchiveCategory(Category category);
    Task DeleteCategory(string id);
  }
}