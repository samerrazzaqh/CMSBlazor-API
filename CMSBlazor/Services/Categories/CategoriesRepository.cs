using System;
using CMSBlazor.Data;
using CMSBlazor.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace CMSBlazor.Services.Categories
{
    public class CategoriesRepository : ICategoriesRepository
    {
        //DataBase==================================
        private readonly AppDbContext appDbContext;

        public CategoriesRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        //====================GetCategories=======================
        public async Task<IEnumerable<Category>?> GetCategories()
        {
            return await appDbContext.Categories.ToListAsync();
        }
        //====================AddCategory=======================
        public async Task<Category?> AddCategory(Category category)
        {
            var result = await appDbContext.Categories.AddAsync(category);
            await appDbContext.SaveChangesAsync();
            return result.Entity;
        }
        //====================DeleteCategory=======================
        public async Task<Category?> DeleteCategory(long categoryId)
        {
            var result = await appDbContext.Categories
                .FirstOrDefaultAsync(e => e.CategoryId == categoryId);
            if (result != null)
            {
                appDbContext.Categories.Remove(result);
                await appDbContext.SaveChangesAsync();
                return result;
            }

            return null;
        }
        //====================GetCategory=======================

        public async Task<Category?> GetCategory(int categoryId)
        {
            return await appDbContext.Categories
                //.Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.CategoryId == categoryId);
        }
        //====================GetCategoryName=======================

        public async Task<Category?> GetCategoryName(Category categoryName)
        {
            return await appDbContext.Categories
                //.Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.CatName == categoryName.CatName);
        }

        //====================UpdateCategory=======================

        public async Task<Category?> UpdateCategory(Category category)
        {
            var result = await appDbContext.Categories
                .FirstOrDefaultAsync(e => e.CategoryId == category.CategoryId);

            if (result != null)
            {
                //result.PostId = post.PostId;
                //result.Auther = post.AboutUsers.ApplicationUsers.UserName;
                result.CatName = category.CatName;

                await appDbContext.SaveChangesAsync();

                return result;
            }

            return null;
        }
        //====================Search=======================
        public async Task<IEnumerable<Category>?> Search(string search)
        {
            IQueryable<Category> query = appDbContext.Categories;

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(e => e.CatName!.Contains(search));
            }

            return await query.ToListAsync();
        }



    }
}

