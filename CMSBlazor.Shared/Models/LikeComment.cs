using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CMSBlazor.Shared.Models
{
    public class LikeComment
    {
        public long LikeCommentId { get; set; }


        [Display(Name = "LikComment"), Required(ErrorMessage = "LikComment is required")]
        public int LikComment { get; set; }



        [Display(Name = "Post"), Required(ErrorMessage = "Post is required")]
        public long PostId { get; set; }

        [Display(Name = "Usename"), Required(ErrorMessage = "Usename is required")]
        public string? AboutUserId { get; set; }
        [Display(Name = "CommentId"), Required(ErrorMessage = "CommentId is required")]
        public long CommentId { get; set; }




        [ForeignKey("CommentId")]
        public Comment? Comments { get; set; }

        [ForeignKey("AboutUserId")]
        public AboutUser? AboutUsers { get; set; }

        [ForeignKey("PostId")]
        public Post? Posts { get; set; }
    }
}

