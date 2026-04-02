using System;
namespace CMSBlazor.Shared.ViewModels.Administration
{
    public class UserRoleViewModel
    {
        public string? RoleId { get; set; }

        public string? UserId { get; set; }

        public List<UserRole>? userRoles { get; set; }
        public List<RolesUser>? rolesUsers { get; set; }


        public string? Message { get; set; }
        public bool Successful { get; set; }
    }



    public class EditUserRoleViewModel
    {
        public string? RoleId { get; set; }

        public string? UserId { get; set; }

        public List<UserRole>? userRoles { get; set; }
        public List<RolesUser>? rolesUsers { get; set; }
    }



    public class UserRole
    {

        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public bool IsSelected { get; set; }
    }

    public class RolesUser
    {
        public string? RoleId { get; set; }
        public string? RoleName { get; set; }
        public bool IsSelected { get; set; }

        public string? Message { get; set; }
        public bool Successful { get; set; }
    }




    public class UserID
    {
        public string? UserId { get; set; }

        public string? Message { get; set; }
        public bool Successful { get; set; }
    }

}

