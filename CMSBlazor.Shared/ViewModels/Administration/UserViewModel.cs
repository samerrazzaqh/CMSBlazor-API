using System;
using System.ComponentModel.DataAnnotations;

namespace CMSBlazor.Shared.ViewModels.Administration
{
    public class UserViewModel
    {
        public UserViewModel()
        {
            Claims = new List<string>();
            Roles = new List<string>();
        }

        public string? Id { get; set; }

        [Required]
        public string? UserName { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        public bool Block { get; set; }

        public List<string> Claims { get; set; }

        public List<string> Roles { get; set; }


        public string? Message { get; set; }
        public bool Successful { get; set; }
    }





    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Claims = new List<string>();
            Roles = new List<string>();
        }

        public string? Id { get; set; }

        [Required]
        public string? UserName { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        public bool Block { get; set; }

        public List<string> Claims { get; set; }

        public List<string> Roles { get; set; }


        public string? Message { get; set; }
        public bool Successful { get; set; }
    }




}

