using System;
using System.ComponentModel.DataAnnotations;
//using Microsoft.AspNetCore.Authentication;

namespace CMSBlazor.Shared.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required]
        public string? EmailOrUserName { get; set; }


        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }


        public string? Message { get; set; }
        public bool Successful { get; set; }
        public string? Token { get; set; }

        public string? ReturnUrl { get; set; }

        // AuthenticationScheme is in Microsoft.AspNetCore.Authentication namespace
        //public IList<AuthenticationScheme>? ExternalLogins { get; set; }
    }
}

