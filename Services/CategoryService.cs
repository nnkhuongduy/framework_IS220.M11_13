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
  public class CategoryService : ICategoryService
  {
    public CategoryService()
    {
      Task.Run(async () =>
      {
        await DB.Index<Category>()
          .Key(user => user.Name, KeyType.Ascending)
          .Option(option => option.Unique = true)
          .CreateAsync();
      }).GetAwaiter().GetResult();
    }

    public async Task<Category> ArchiveCategory(Category category)
    {
      category.Status = CategoryStatus.ARCHIVED;

      await category.SaveAsync();

      if (category.CategoryLevel == CategoryLevel.SECONDARY)
      {
        var parentCategories = await DB.Find<Category>().Match(_ => _.SubCategories.Contains(ObjectId.Parse(category.ID))).ExecuteAsync();

        foreach (var _ in parentCategories)
        {
          _.SubCategories.Remove(ObjectId.Parse(category.ID));
          await _.SaveAsync();
        }
      }

      return category;
    }

    public async Task<Category> CreateCategory(PostCategoryBody body)
    {
      Category category = new Category();

      category.Name = body.Name;
      category.Image = body.Image;
      category.CategoryLevel = body.CategoryLevel;
      category.Status = body.Status;
      category.Specs = new List<Spec>();
      category.SubCategories = new List<ObjectId>();

      await category.SaveAsync();

      return category;
    }

    public async Task DeleteCategory(string id)
    {
      var deletingCategory = await DB.Find<Category>().MatchID(id).ExecuteFirstAsync();

      if (deletingCategory == null)
      {
        throw new HttpError(false, 404, "Danh mục không tìm thấy!");
      }

      await deletingCategory.DeleteAsync();

      return;
    }

    public async Task<Category> GetCategory(string id)
    {
      var result = await DB.Find<Category>().Match(_ => _.ID == id).ExecuteFirstAsync();

      if (result == null)
      {
        throw new HttpError(false, 404, "Không tìm thấy danh mục sản phẩm");
      }

      return result;
    }

    public async Task<Category> UpdateCategory(PutCategoryBody body, string id)
    {
      List<ObjectId> subCategories;

      try
      {
        subCategories = await DB.Find<Category, ObjectId>().Match(_ => _.In(_ => _.ID, body.SubCategories)).Project(_ => ObjectId.Parse(_.ID)).ExecuteAsync();
      }
      catch (Exception)
      {
        throw new HttpError(false, 400, "Không tìm thấy danh mục con");
      }

      var newCategory = await DB.UpdateAndGet<Category>()
        .MatchID(id)
        .Modify(_ => _.Name, body.Name)
        .Modify(_ => _.Image, body.Image)
        .Modify(_ => _.CategoryLevel, body.CategoryLevel)
        .Modify(_ => _.Status, body.Status)
        .Modify(_ => _.SubCategories, subCategories)
        .ExecuteAsync();

      return newCategory;
    }
  }
}