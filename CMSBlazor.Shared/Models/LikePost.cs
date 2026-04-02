using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CMSBlazor.Shared.Models
{
    public class LikePost
    {


        public long LikePostId { get; set; }


        [Display(Name = "LikPost"), Required(ErrorMessage = "LikeonPost is required")]
        public int LikPost { get; set; }



        [Display(Name = "Post"), Required(ErrorMessage = "Post is required")]
        public long PostId { get; set; }

        [Display(Name = "Usename"), Required(ErrorMessage = "Usename is required")]
        public string? AboutUserId { get; set; }



        [ForeignKey("AboutUserId")]
        public AboutUser? AboutUsers { get; set; }

        [ForeignKey("PostId")]
        public Post? Posts { get; set; }

    }
}

