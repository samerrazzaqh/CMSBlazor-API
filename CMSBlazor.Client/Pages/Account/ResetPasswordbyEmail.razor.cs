using System;
using CMSBlazor.Client.Service.Account;
using CMSBlazor.Shared.ViewModels.Account;
using Microsoft.AspNetCore.Components;

namespace CMSBlazor.Client.Pages.Account
{
	public partial class ResetPasswordbyEmail
	{

        [Inject]
        public IAccountService? accountService { get; set; }
        public ResetPasswordbyEmailViewModel ResetPasswordbyEmailViewModel = new ResetPasswordbyEmailViewModel();
        public bool ShowMessage;
        public string? Message;
        [Parameter]
        public string? Email { get; set; }
        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        public async Task HandleResetPasswordbyEmail()
        {
            ShowMessage = false;
            //===============Decode==================================
            var FromBase64String = Convert.FromBase64String(Email!);
            var UTF8_Email = System.Text.Encoding.UTF8.GetString(FromBase64String);
            //=================================================
            ResetPasswordbyEmailViewModel.Email = UTF8_Email;
            var result = await accountService!.ResetPasswordbyEmail(ResetPasswordbyEmailViewModel);

            if (result!.Successful)
            {

                NavigationManager!.NavigateTo("/login");
            }
            else
            {
                Message = result.Message;
                ShowMessage = true;
            }
            StateHasChanged();
        }















    }
}

