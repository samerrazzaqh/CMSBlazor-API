using System;
namespace CMSBlazor.Shared.ViewModels.Administration
{
    public class UserClaimsViewModel
    {
        public UserClaimsViewModel()
        {
            Cliams = new List<UserClaim>();
        }

        public string? UserId { get; set; }
        public List<UserClaim> Cliams { get; set; }

        public string? Message { get; set; }
        public bool Successful { get; set; }
    }


    public class EditUserClaimsViewModel
    {
        public EditUserClaimsViewModel()
        {
            Cliams = new List<UserClaim>();
        }

        public string? UserId { get; set; }
        public List<UserClaim> Cliams { get; set; }

        public string? Message { get; set; }
        public bool Successful { get; set; }
    }


    public class UserClaim
    {
        public string? ClaimType { get; set; }
        public bool IsSelected { get; set; }
    }



}

