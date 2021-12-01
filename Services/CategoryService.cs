using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Entities;
using Slugify;

using _99phantram.Entities;
using _99phantram.Interfaces;
using _99phantram.Models;

namespace _99phantram.Services
{
  public class CategoryService : ICategoryService
  {
    public CategoryService()
    {
      Task.Run(async () =>
      {
        await DB.Index<Category>()
          .Key(user => user.Slug, KeyType.Ascending)
          .Option(option => option.Unique = true)
          .CreateAsync();
      }).GetAwaiter().GetResult();
    }

    private async Task _RemoveFromParents(Category category)
    {
      var parents = await DB.Find<Category>().Match(_ => _.SubCategories.Contains(ObjectId.Parse(category.ID))).ExecuteAsync();

      foreach (var _ in parents)
      {
        _.SubCategories.Remove(ObjectId.Parse(category.ID));
        await _.SaveAsync();
      }
    }

    public async Task<Category> ArchiveCategory(Category category)
    {
      var supplies = await DB
        .Find<Supply>()
        .Match(_ => _.Status == SupplyStatus.ARCHIVED)
        .Match(_ => _.ElemMatch(__ => __.Categories, __ => __.ID == category.ID))
        .ExecuteAsync();

      if (supplies.Count > 0)
      {
        throw new HttpError(false, 400, "Không thể lưu trữ danh mục vẫn còn sản phẩm đang sử dụng!");
      }

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
      var slugHelper = new SlugHelper();

      category.Name = body.Name;
      category.Image = body.Image;
      category.CategoryLevel = body.CategoryLevel;
      category.Status = body.Status;
      category.Specs = new List<Spec>();
      category.SubCategories = new List<ObjectId>();
      category.Slug = slugHelper.GenerateSlug(body.Slug);

      await category.SaveAsync();

      return category;
    }

    public async Task DeleteCategory(string id)
    {
      var category = await GetCategory(id);

      if (category.Status != CategoryStatus.ARCHIVED)
      {
        throw new HttpError(false, 404, "Danh mục không thể xóa!");
      }

      await category.DeleteAsync();
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
      var slugHelper = new SlugHelper();
      var supplies = await DB
        .Find<Supply>()
        .Match(_ => _.Status != SupplyStatus.ARCHIVED)
        .Match(_ => _.ElemMatch(__ => __.Categories, __ => __.ID == id))
        .ExecuteAsync();

      var updatingCategory = await GetCategory(id);

      try
      {
        subCategories = await DB.Find<Category, ObjectId>().Match(_ => _.In(_ => _.ID, body.SubCategories)).Project(_ => ObjectId.Parse(_.ID)).ExecuteAsync();
      }
      catch (Exception)
      {
        throw new HttpError(false, 400, "Không tìm thấy danh mục con");
      }

      if (updatingCategory.CategoryLevel != body.CategoryLevel)
      {
        if (updatingCategory.CategoryLevel == CategoryLevel.PRIMARY && updatingCategory.SubCategories.Count > 0)
          throw new HttpError(false, 400, "Không thể đổi cấp danh mục khi vẫn còn danh mục con!");

        if (updatingCategory.CategoryLevel == CategoryLevel.SECONDARY && supplies.Count > 0)
          throw new HttpError(false, 400, "Không thể đổi cấp danh mục khi vẫn còn sản phẩm sử dụng danh mục!");
        
        await _RemoveFromParents(updatingCategory);
        subCategories = new List<ObjectId>();
      }

      if (body.Status == CategoryStatus.ARCHIVED)
      {
        updatingCategory = await ArchiveCategory(updatingCategory);
      }

      updatingCategory.Name = body.Name;
      updatingCategory.Image = body.Image;
      updatingCategory.CategoryLevel = body.CategoryLevel;
      updatingCategory.Status = body.Status;
      updatingCategory.SubCategories = subCategories;
      updatingCategory.Slug = slugHelper.GenerateSlug(body.Slug);

      await updatingCategory.SaveAsync();

      foreach (var supply in supplies)
      {
        var category = supply.Categories.Find(_ => _.ID == id);

        category.Name = body.Name;
        category.Image = body.Image;
        category.CategoryLevel = body.CategoryLevel;
        category.Status = body.Status;
        category.Slug = slugHelper.GenerateSlug(body.Slug);

        await supply.SaveAsync();
      }

      return updatingCategory;
    }
  }
}