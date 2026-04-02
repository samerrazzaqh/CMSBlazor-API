using System;
using System.ComponentModel.DataAnnotations;

namespace CMSBlazor.Shared.ViewModels.Profile
{
    public class EditAccount
    {
        public string? UserId { get; set; }
        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        [Required]
        public string? UserName { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        //[Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }



        public string? Message { get; set; }
        public bool Successful { get; set; }
    }






    public class AddPassword
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage =
            "The new password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
    }



}

