using System;
using CMSBlazor.Shared.ViewModels.Post;

namespace CMSBlazor.Client.Service.Categories
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryViewModel>?> GetCategories();
        Task<CategoryViewModel?> GetCategory(int id);
        Task<CategoryViewModel?> UpdateCategory(CategoryViewModel updatedCategory);
        Task<CategoryViewModel> CreateCategory(CategoryViewModel newCategory);
        Task DeleteCategory(int id);
    }
}

