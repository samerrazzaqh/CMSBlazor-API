using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CMSBlazor.Shared.Models
{
    public class Comment
    {
        public long CommentId { get; set; }

        [Display(Name = "TextComment"), Required(ErrorMessage = "Comment is required")]
        public string? TextComment { get; set; }

        [Display(Name = "Date LCO"), Required(ErrorMessage = "Date is LCO")]
        public DateTime LCODate { get; set; }



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

