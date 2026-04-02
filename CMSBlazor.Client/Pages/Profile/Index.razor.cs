using System;
using AutoMapper;
using Blazored.LocalStorage;
using CMSBlazor.Client.Helpers;
using CMSBlazor.Client.Models;
using CMSBlazor.Client.Service.Account;
using CMSBlazor.Client.Service.Profile;
using CMSBlazor.Shared.ViewModels.Profile;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace CMSBlazor.Client.Pages.Profile
{
	public partial class Index
	{
        [Inject]
        public IAccountService? accountService { get; set; }

        [Inject]
        public IProfileService? profileService { get; set; }

        public InfoProfile infoProfile { get; set; } = new InfoProfile();

        [Inject]
        public IMapper? Mapper { get; set; }


        public InfoProfile? InfoProfile { get; set; } = new InfoProfile();

        public EditProfile? EditProfile { get; set; } = new EditProfile();


        public bool ShowMessage;
        public string? Message;

        public LocalStorageModel? localStorageModel { get; set; }

        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        [Inject]
        public ILocalStorageService? localStorageService { get; set; }

        public string? pathImageProfile, ImageProfileOld, pathImageCover, ImageCoverOld;
        private bool isVisible;

        protected override async Task OnInitializedAsync()
        {
            isVisible = false;

            localStorageModel = await localStorageService!.GetItemAsync<LocalStorageModel>("Storage");

            var result = await profileService!.GetInfoProfile(localStorageModel.UserId!);
            infoProfile.UserName = result!.UserName;
            infoProfile.Email = result.Email;
            infoProfile.FirstName = result.FirstName;
            infoProfile.LastName = result.LastName;
            infoProfile.Location = result.Location;
            infoProfile.PhoneNumber = result.PhoneNumber;
            infoProfile.Profession = result.Profession;
            infoProfile.DateOfBurthEdite = result.DateOfBurthEdite;
            infoProfile.Skills = result.Skills;

            //=========ImageProfile============
            if (result.UrlImageProfile != null) {
                infoProfile.UrlImageProfile =CMSConstant.BaseApiAddress+ $"/profile/{result.UrlImageProfile}";
                pathImageProfile = infoProfile.UrlImageProfile;
            } else { infoProfile.UrlImageProfile = result.UrlImageProfile; }
            

            //=========ImageCover============
            if (result.UrlImageCover != null)
            {
                infoProfile.UrlImageCover = CMSConstant.BaseApiAddress+ $"/profile/{result.UrlImageCover}";
                pathImageCover = infoProfile.UrlImageCover;
            } else { infoProfile.UrlImageCover = result.UrlImageCover; }



            infoProfile.WorkLink = result.WorkLink;
            infoProfile.Experience = result.Experience;
            infoProfile.UserId = result.UserId;
        }

        private async Task GoEditeInfo()
        {
            var result = await profileService!.GetInfoProfile(localStorageModel!.UserId!);
            infoProfile.UserId = result!.UserId;
            NavigationManager!.NavigateTo($"/editinfoprofile/{infoProfile.UserId}");
        }

        private async Task GoEditeAccount()
        {
            var result = await profileService!.GetInfoProfile(localStorageModel!.UserId!);
            infoProfile.UserId = result!.UserId;
            NavigationManager!.NavigateTo($"/editinfoaccount/{infoProfile.UserId}");
        }


        //IList<IBrowserFile> files1 = new List<IBrowserFile>();
        public async Task UploadImageProfile(IBrowserFile filep)
        {
            isVisible = true;
            //files1.Add(file);
            localStorageModel = await localStorageService!.GetItemAsync<LocalStorageModel>("Storage");
            //read that file in a byte array
            var bufferp = new byte[filep.Size];
            await filep.OpenReadStream(long.MaxValue).ReadAsync(bufferp);
            //convert byte array to base 64 string
            pathImageProfile = $"data:image/png;base64,{Convert.ToBase64String(bufferp)}";

            //===========================Image Edite======================================
            InfoProfile = await profileService!.GetInfoProfile(localStorageModel.UserId!);
            ImageProfileOld = InfoProfile!.UrlImageProfile;
            Mapper!.Map(InfoProfile, EditProfile);


            EditProfile!.UrlImageCover = null;
            EditProfile!.UrlImageCoverName = null;
            EditProfile!.UrlImageCoverNameOld = null;

            EditProfile!.UrlImageProfile = Convert.ToBase64String(bufferp);
            EditProfile.UrlImageProfileName = filep.Name;
            EditProfile!.UrlImageProfileNameOld = ImageProfileOld;
            Mapper.Map(EditProfile, InfoProfile);
            await profileService.EditInfoProfile(InfoProfile!);

            isVisible = false;
        }


        //IList<IBrowserFile> files2 = new List<IBrowserFile>();
        public async Task UploadImageCover(IBrowserFile filec)
        {
            isVisible = true;
            //files2.Add(file);
            localStorageModel = await localStorageService!.GetItemAsync<LocalStorageModel>("Storage");
            //read that file in a byte array
            var bufferc = new byte[filec.Size];
            await filec.OpenReadStream(long.MaxValue).ReadAsync(bufferc);
            //convert byte array to base 64 string
            pathImageCover = $"data:image/png;base64,{Convert.ToBase64String(bufferc)}";

            //===========================Image Edite======================================
            InfoProfile = await profileService!.GetInfoProfile(localStorageModel.UserId!);
            ImageCoverOld = InfoProfile!.UrlImageCover;
            Mapper!.Map(InfoProfile, EditProfile);


            EditProfile!.UrlImageProfile = null;
            EditProfile.UrlImageProfileName = null;
            EditProfile!.UrlImageProfileNameOld = null;


            EditProfile!.UrlImageCover = Convert.ToBase64String(bufferc);
            EditProfile!.UrlImageCoverName = filec.Name;
            EditProfile!.UrlImageCoverNameOld = ImageCoverOld;
            Mapper.Map(EditProfile, InfoProfile);
            await profileService.EditInfoProfile(InfoProfile!);
            isVisible = false;
        }




    }
}

