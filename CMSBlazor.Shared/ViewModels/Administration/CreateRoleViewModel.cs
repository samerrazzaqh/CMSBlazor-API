using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CMSBlazor.Shared.ViewModels.Administration
{
    public class CreateRoleViewModel
    {
        [Required]
        [Display(Name = "Role")]
        public string? RoleName { get; set; }


        public string? Message { get; set; }
        public bool Successful { get; set; }
    }
}

