using System;
using CMSBlazor.Shared.Models;

namespace CMSBlazor.Services.Categories
{
    public interface ICategoriesRepository
    {
        Task<IEnumerable<Category>?> Search(string search);
        Task<IEnumerable<Category>?> GetCategories();
        Task<Category?> GetCategory(int categoryId);
        Task<Category?> GetCategoryName(Category categoryName);
        Task<Category?> AddCategory(Category category);
        Task<Category?> UpdateCategory(Category category);
        Task<Category?> DeleteCategory(long categoryId);
    }
}

