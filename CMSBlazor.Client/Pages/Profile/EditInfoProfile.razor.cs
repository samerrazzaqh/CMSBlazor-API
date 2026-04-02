using System;
using System.Net;
using AutoMapper;
using CMSBlazor.Client.Service.Profile;
using CMSBlazor.Shared.ViewModels.Profile;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace CMSBlazor.Client.Pages.Profile
{
	public partial class EditInfoProfile
	{

        [Parameter]
        public string? UserId { get; set; }

        [Inject]
        public IMapper? Mapper { get; set; }


        public InfoProfile? InfoProfile { get; set; } = new InfoProfile();

        public EditProfile? EditProfile { get; set; } = new EditProfile();

        [Inject]
        public IProfileService? profileService { get; set; }

        
        public bool ShowMessage;
        public bool Successful;
        public string? Message;

        [Inject]
        public NavigationManager? NavigationManager { get; set; }


        protected async override Task OnInitializedAsync()
        {
            InfoProfile = await profileService!.GetInfoProfile(UserId!);
            Mapper!.Map(InfoProfile, EditProfile);
        }




        protected async Task HandleValidSubmit()
        {
            Mapper!.Map(EditProfile, InfoProfile);
            InfoProfile!.UrlImageProfile = null;
            InfoProfile!.UrlImageCover = null;
            var result = await profileService!.EditInfoProfile(InfoProfile!);

            if (result!.Successful)
            {
                NavigationManager!.NavigateTo("javascript:history.back()"); 
            }
            else
            {
                Successful = result.Successful;
            }

        }




    }
}

