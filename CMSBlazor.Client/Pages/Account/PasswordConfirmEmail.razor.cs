using System;
using CMSBlazor.Client.Service.Account;
using CMSBlazor.Shared.ViewModels.Account;
using Microsoft.AspNetCore.Components;

namespace CMSBlazor.Client.Pages.Account
{
	public partial class PasswordConfirmEmail
	{
        [Inject]
        public IAccountService? accountService { get; set; }
        public ConfirmEmailViewModel PasswordConfirmEmailViewModel = new ConfirmEmailViewModel();
        public bool ShowMessage;
        public string? Message;
        [Parameter]
        public string? Email { get; set; }
        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        public async Task HandlePasswordConfirmEmail()
        {
            ShowMessage = false;
            //===============Decode==================================
            var FromBase64String = Convert.FromBase64String(Email!);
            var UTF8_Email = System.Text.Encoding.UTF8.GetString(FromBase64String);
            //=================================================
            PasswordConfirmEmailViewModel.EmailOrUserName = UTF8_Email;
            var result = await accountService!.PasswordConfirmEmail(PasswordConfirmEmailViewModel);

            if (result!.Successful)
            {

                NavigationManager!.NavigateTo($"/resetpasswordbyemail/{Email}");
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

