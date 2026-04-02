using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMSBlazor.Shared.Models
{
    public class Post
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PostId { get; set; }

        [StringLength(100), Display(Name = "Title"), Required(ErrorMessage = "title is required")]
        public string? Title { get; set; }

        [Display(Name = "Subject"), Required(ErrorMessage = "Subject is required")]
        public string? PostContent { get; set; }

        [Display(Name = "LinkVideo"), StringLength(200), DataType(DataType.Url)]
        public string? LinkVideo { get; set; }

        [Display(Name = "Image")]
        public string? PostImg { get; set; }

        public string? Auther { get; set; }

        [Display(Name = "Date Subject"), Required(ErrorMessage = "Date is required")]
        public DateTime PostDate { get; set; }

        [Display(Name = "Views")]
        public int PostViews { get; set; }

        [Display(Name = "Category"), Required(ErrorMessage = "Category is required")]
        public int Category { get; set; }

        [Display(Name = "Publish")]
        public bool IsPublish { get; set; }





        [ForeignKey("Auther")]
        public AboutUser? AboutUsers { get; set; }


        [ForeignKey("Category")]
        public Category? Categories { get; set; }
    }
}

