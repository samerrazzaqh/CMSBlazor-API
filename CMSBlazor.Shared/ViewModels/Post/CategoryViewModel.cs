using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CMSBlazor.Shared.ViewModels.Post
{
    public class CategoryViewModel
    {
        public int CategoryId { get; set; }

        [StringLength(50), Display(Name = "Category Name"), Required(ErrorMessage = "Category is required")]
        public string? CatName { get; set; }




        public string? Message { get; set; }
        public bool Successful { get; set; }
    }



    public class EditeCategoryModel
    {
        public int CategoryId { get; set; }

        [StringLength(50), Display(Name = "Category Name"), Required(ErrorMessage = "Category is required")]
        public string? CatName { get; set; }




        public string? Message { get; set; }
        public bool Successful { get; set; }
    }



}

