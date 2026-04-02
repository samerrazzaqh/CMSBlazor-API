using System;
namespace CMSBlazor.Shared.ViewModels.Administration
{
	public class Users
    {
        public string? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }








        //"block": false,
        //"blockTime": null,
        //"emailConfirmed": true,
        //"passwordHash": null,
        //"phoneNumber": null,
        //"phoneNumberConfirmed": false,
        //"twoFactorEnabled": false,
        //"lockoutEnd": null,
        //"lockoutEnabled": true,

        public string? Message { get; set; }
        public bool Successful { get; set; }

    }
}

