using System;
using CMSBlazor.Services.Categories;
using CMSBlazor.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMSBlazor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoriesRepository categoriesRepository;

        public CategoryController(ICategoriesRepository categoriesRepository)
        {
            this.categoriesRepository = categoriesRepository;
        }
        //==========================GetCategories============================
        [HttpGet]
        public async Task<ActionResult> GetCategories()
        {
            try
            {
                return Ok(new { categories = await categoriesRepository.GetCategories() });
                //return Ok(await categoriesRepository.GetCategories());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }
        //==========================CreateCategory============================
        [HttpPost]
        public async Task<ActionResult<Category?>> CreateCategory(Category category)
        {
            try
            {
                if (category == null)
                {
                    return BadRequest();
                }

                var getcategoryname = await categoriesRepository.GetCategoryName(category);
                if(getcategoryname != null)
                {
                    return Ok(new { success = false, message = "Category '" + category.CatName + "' is already taken." });
                }
                var createdCategory = await categoriesRepository.AddCategory(category);
                return Ok(new { success = true, message = "Category '" + category.CatName + "' added new" });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }
        //==========================Search============================
        [HttpGet("{search}")]
        public async Task<ActionResult<IEnumerable<Category>>> Search(string search)
        {
            try
            {
                var result = await categoriesRepository.Search(search);

                if (result!.Any())
                {
                    return Ok(result);
                }

                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving data from the database");
            }
        }

        //==========================GetCategory============================
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            try
            {
                var result = await categoriesRepository.GetCategory(id);

                if (result == null)
                {
                    return NotFound();
                }

                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }
        //==========================UpdateCategory============================
        [HttpPut()]
        public async Task<ActionResult<Category?>> UpdateCategory(Category category)
        {
            try
            {
                var categoryToUpdate = await categoriesRepository.GetCategory(category.CategoryId);

                if (categoryToUpdate == null)
                {
                    return NotFound($"Category with Id = {category.CategoryId} not found");
                }

                var getcategoryname = await categoriesRepository.GetCategoryName(category);
                if (getcategoryname != null)
                {
                    return Ok(new { success = false, message = "Category '" + category.CatName + "' is already taken." });
                }

                var updateCategory = await categoriesRepository.UpdateCategory(category);
                return Ok(new { success = true, message = "Category '" + category.CatName + "' updated" });


            }
            catch (Exception)
            {
                return Ok(new { success = false, message = "Error updating data"});
            }
        }
        //==========================DeleteCategory============================
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Category?>> DeleteCategory(int id)
        {
            try
            {
                var categoryToDelete = await categoriesRepository.GetCategory(id);

                if (categoryToDelete == null)
                {
                    return Ok(new { success = false, message = $"Category not found" });
                }
                else
                {
                    try
                    {
                        await categoriesRepository.DeleteCategory(id);
                        return Ok(new { success = true, message = "Delete This Category" });
                    }
                    catch
                    { 
                        return Ok(new { success = false, message = $"This category cannot be deleted because it contains a post. If you want to delete it, you must delete the post associated with it." });
                    }
                }
                

            }
            catch (Exception)
            {
                return Ok(new { success = false, message = "Error deleting data" });
            }
        }

    }
}

