using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMSBlazor.Shared.Models
{
    public class AboutUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? AboutUserId { get; set; }
        public string? Profession { get; set; }
        [Display(Name = "BirthDay"), DataType(DataType.Date)]
        public DateTime? DateOfBurth { get; set; }
        [Display(Name = "City"), StringLength(100)]
        public string? Location { get; set; }
        public string? Skills { get; set; }
        public string? WorkLink { get; set; }
        public string? Experience { get; set; }
        public string? UrlImageProfile { get; set; }
        public string? UrlImageCover { get; set; }



        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser? ApplicationUsers { get; set; }
    }
}

