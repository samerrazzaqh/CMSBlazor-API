using System;
namespace CMSBlazor.Shared.ViewModels.Administration
{
	public class PolicyRole
	{
        public PolicyRole()
        {
            roles = new List<Role>();
            policies = new List<Policy>();
        }


        public string? UserId { get; set; }

        public List<Role>? roles { get; set; }
        public List<Policy>? policies { get; set; }



        public string? Message { get; set; }
        public bool Successful { get; set; }

    }

    public class Role
    {

        public string? RoleName { get; set; }
    }

    public class Policy
    {

        public string? ClaimType { get; set; }
        public bool ClaimValue { get; set; }
    }



}

