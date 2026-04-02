using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using System.ComponentModel.DataAnnotations;

namespace CMSBlazor.Shared.Models
{
    public class LikeReComment
    {

        public long LikeReCommentId { get; set; }


        [Display(Name = "LikReComment"), Required(ErrorMessage = "LikReComment is required")]
        public int LikReComment { get; set; }



        [Display(Name = "Post"), Required(ErrorMessage = "Post is required")]
        public long PostId { get; set; }

        [Display(Name = "Usename"), Required(ErrorMessage = "Usename is required")]
        public string? AboutUserId { get; set; }

        [Display(Name = "CommentId"), Required(ErrorMessage = "CommentId is required")]
        public long CommentId { get; set; }

        [Display(Name = "RrCommentId"), Required(ErrorMessage = "ReCommentId is required")]
        public long ReCommentId { get; set; }



        [ForeignKey("ReCommentId")]
        public ReComment? ReComments { get; set; }

        [ForeignKey("CommentId")]
        public Comment? Comments { get; set; }

        [ForeignKey("AboutUserId")]
        public AboutUser? AboutUsers { get; set; }

        [ForeignKey("PostId")]
        public Post? Posts { get; set; }

    }
}

