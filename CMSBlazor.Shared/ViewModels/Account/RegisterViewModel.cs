using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using CMSBlazor.Shared.CustomValidators;

namespace CMSBlazor.Shared.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [EmailDomainValidator(AllowedDomain = "gmail.com",
            ErrorMessage = "Only gmail.com is allowed")]
        public string? Email { get; set; }

        [Required]
        public string? UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password",
            ErrorMessage = "Password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? Message { get; set; }
        public bool Successful { get; set; }

    }
}

