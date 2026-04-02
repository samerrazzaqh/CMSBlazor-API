using System;
namespace CMSBlazor.Shared.ViewModels.Profile
{
    public class InfoProfile
    {
        public string? UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? Profession { get; set; }

        public string? UrlImageProfile { get; set; }
        public string? UrlImageProfileName { get; set; }
        public string? UrlImageProfileNameOld { get; set; }


        public string? UrlImageCover { get; set; }
        public string? UrlImageCoverName { get; set; }
        public string? UrlImageCoverNameOld { get; set; }


        public string? Location { get; set; }
        public string? Skills { get; set; }
        public string? WorkLink { get; set; }
        public string? Experience { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
        public DateTime DateOfBurthEdite { get; set; }



        public string? Message { get; set; }
        public bool Successful { get; set; }

    }



    public class EditProfile
    {
        public string? UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? Profession { get; set; }

        public string? UrlImageProfile { get; set; }
        public string? UrlImageProfileName { get; set; }
        public string? UrlImageProfileNameOld { get; set; }


        public string? UrlImageCover { get; set; }
        public string? UrlImageCoverName { get; set; }
        public string? UrlImageCoverNameOld { get; set; }


        public string? Location { get; set; }
        public string? Skills { get; set; }
        public string? WorkLink { get; set; }
        public string? Experience { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
        public DateTime? DateOfBurthEdite { get; set; }



        public string? Message { get; set; }
        public bool Successful { get; set; }

    }



}

