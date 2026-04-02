using System;
using System.ComponentModel.DataAnnotations;

namespace CMSBlazor.Shared.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [StringLength(50), Display(Name = "Category Name"), Required(ErrorMessage = "Category is required")]
        public string? CatName { get; set; }
    }
}

