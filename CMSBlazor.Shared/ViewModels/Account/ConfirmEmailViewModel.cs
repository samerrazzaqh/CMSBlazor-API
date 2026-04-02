using System;
using System.ComponentModel.DataAnnotations;

namespace CMSBlazor.Shared.ViewModels.Account
{
	public class ConfirmEmailViewModel
	{
        //[Required]
        public string? EmailOrUserName { get; set; }

        [Required]
        public string? TokenFromEmail { get; set; }

        public string? Message { get; set; }
        public bool Successful { get; set; }
    }
}

