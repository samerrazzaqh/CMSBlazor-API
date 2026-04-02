using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using CMSBlazor.Shared.Models;
using CMSBlazor.Shared.ViewModels.Administration;

namespace CMSBlazor.Shared.ViewModels.Post
{
    public class PostAll
    {
        public PostAll()
        {
            PostViewModels = new List<PostViewModel>();
        }

        public List<PostViewModel>? PostViewModels { get; set; }

        public int PostsCount { get; set; }
    }


    public class PostViewModel
    {
        public long PostId { get; set; }

        public string? Auther { get; set; }

        [StringLength(100), Display(Name = "Title"), Required(ErrorMessage = "title is required")]
        public string? Title { get; set; }

        [Display(Name = "Subject"), Required(ErrorMessage = "Subject is required")]
        public string? PostContent { get; set; }

        [Display(Name = "LinkVideo"), StringLength(200), DataType(DataType.Url)]
        public string? LinkVideo { get; set; }

        [Display(Name = "Image")]
        public string? PostImg { get; set; }
        public string? PostImgName { get; set; }
        public string? PostImgNameOld { get; set; }

        public DateTime PostDate { get; set; }

        public string? CatName { get; set; }

        [Display(Name = "Category"), Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }

        [Display(Name = "Views")]
        public int PostViews { get; set; }
        

        [Display(Name = "Publish")]
        public bool IsPublish { get; set; }




        public List<UsersLikePost>? UsersLikePosts { get; set; }
        public string? UrlImageAuther { get; set; }
        public int NumberComment { get; set; }
        public int NumberLikes { get; set; }


        public string? Message { get; set; }
        public bool Successful { get; set; }
    }


    public class EditPostViewModel
    {
        public long PostId { get; set; }

        public string? Auther { get; set; }

        [StringLength(100), Display(Name = "Title"), Required(ErrorMessage = "title is required")]
        public string? Title { get; set; }

        [Display(Name = "Subject"), Required(ErrorMessage = "Subject is required")]
        public string? PostContent { get; set; }

        [Display(Name = "LinkVideo"), StringLength(200), DataType(DataType.Url)]
        public string? LinkVideo { get; set; }

        [Display(Name = "Image")]
        public string? PostImg { get; set; }
        public string? PostImgName { get; set; }

        public DateTime PostDate { get; set; }

        public string? CatName { get; set; }

        [Display(Name = "Category"), Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }

        [Display(Name = "Views")]
        public int PostViews { get; set; }


        [Display(Name = "Publish")]
        public bool IsPublish { get; set; }



        public string? Message { get; set; }
        public bool Successful { get; set; }
    }


    public class UsersLikePost
    {
        public string? userName { get; set; }
        public string? urlImageProfile { get; set; }
        public string? userId { get; set; }
    }


    public class PostUtilities
    {
        public long PostId { get; set; }
        public int CategoryId { get; set; }
        public int NumGetPost { get; set; }
        
        public string? Search { get; set; }


        public string? Message { get; set; }
        public bool Successful { get; set; }
    }





}

