using System;
using CMSBlazor.Shared.CustomValidators;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CMSBlazor.Shared.ViewModels.Account
{
	public class RegisterExternalViewModel
	{
        public string? Email { get; set; }

        public string? UserName { get; set; }
        public string? Token { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? Message { get; set; }
        public bool Successful { get; set; }
    }
}

