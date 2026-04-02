using System;
using AutoMapper;
using CMSBlazor.Client.Service.Profile;
using CMSBlazor.Shared.ViewModels.Profile;
using Microsoft.AspNetCore.Components;

namespace CMSBlazor.Client.Pages.Profile
{
	public partial class EditInfoAccount
	{
        [Parameter]
        public string? UserId { get; set; }

        [Inject]
        public IMapper? Mapper { get; set; }


        public InfoProfile? InfoProfile { get; set; } = new InfoProfile();

        public EditAccount? EditAccount { get; set; } = new EditAccount();

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
            InfoProfile!.Password = "";
            Mapper!.Map(InfoProfile, EditAccount);
        }




        protected async Task HandleValidSubmit()
        {
            InfoProfile = await profileService!.GetInfoProfile(UserId!);
            InfoProfile!.Password = InfoProfile.Password;
            Mapper!.Map(EditAccount, InfoProfile);
            EditAccount? result = null;
            result = await profileService.EditAccount(EditAccount!);

            if (result!.Successful)
            {
                NavigationManager!.NavigateTo("javascript:history.back()");
            }
            else
            {
                ShowMessage = true;
                Message = result.Message;
                Successful = result.Successful;
            }

        }


    }
}

