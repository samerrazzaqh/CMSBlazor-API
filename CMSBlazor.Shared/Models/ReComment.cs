using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMSBlazor.Shared.Models
{
    public class ReComment
    {

        public long ReCommentId { get; set; }

        [Display(Name = "ReTextComment"), Required(ErrorMessage = "ReComment is required")]
        public string? ReTextComment { get; set; }

        [Display(Name = "Date LCO"), Required(ErrorMessage = "Date is LCO")]
        public DateTime LCODate { get; set; }



        [Display(Name = "Post"), Required(ErrorMessage = "Post is required")]
        public long PostId { get; set; }

        [Display(Name = "Usename"), Required(ErrorMessage = "Usename is required")]
        public string? AboutUserId { get; set; }

        [Display(Name = "ReComment"), Required(ErrorMessage = "ReComment is required")]
        public long CommentId { get; set; }




        [ForeignKey("CommentId")]
        public Comment? Comments { get; set; }

        [ForeignKey("AboutUserId")]
        public AboutUser? AboutUsers { get; set; }

        [ForeignKey("PostId")]
        public Post? Posts { get; set; }

    }
}

