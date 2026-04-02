using System;
using System.ComponentModel.DataAnnotations;

namespace CMSBlazor.Shared.ViewModels.Administration
{
    public class RoleViewModel
    {
        public RoleViewModel()
        {
            Users = new List<string>();
        }

        public string? Id { get; set; }
        public string? RoleName { get; set; }
        public List<string>? Users { get; set; }



        public string? Message { get; set; }
        public bool Successful { get; set; }
    }


    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<string>();
        }
        public string? Id { get; set; }
        public string? RoleName { get; set; }
        public List<string>? Users { get; set; }



        public string? Message { get; set; }
        public bool Successful { get; set; }
    }

}

