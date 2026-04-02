using System;
using System.ComponentModel.DataAnnotations;

namespace CMSBlazor.Shared.ViewModels.Account
{
    public class SendTokenEmail
    {
        [Required]
        public string? Email { get; set; }


        public string? Message { get; set; }
        public bool Successful { get; set; }
    }
}

