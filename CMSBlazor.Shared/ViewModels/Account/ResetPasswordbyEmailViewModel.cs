using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CMSBlazor.Shared.ViewModels.Account
{
    public class ResetPasswordbyEmailViewModel
    {
        //[Required]
        //[EmailAddress]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("NewPassword",
            ErrorMessage = "Password and Confirm Password must match")]
        public string? ConfirmNewPassword { get; set; }

        //public string? Token { get; set; }

        public string? Message { get; set; }
        public bool Successful { get; set; }
    }
}

