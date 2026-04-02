using System;
using CMSBlazor.Client.Service.Account;
using CMSBlazor.Shared.ViewModels.Account;
using Microsoft.AspNetCore.Components;

namespace CMSBlazor.Client.Pages.Account
{
	public partial class SendConfirmationEmail
	{
        [Inject]
        public IAccountService? accountService { get; set; }
        public SendTokenEmail SendTokenEmail = new SendTokenEmail();
        public bool ShowMessage;
        public string? Message;
        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        public async Task HandleSendConfirmEmail()
        {
            ShowMessage = false;
            var result = await accountService!.SendConfirmationEmail(SendTokenEmail);

            if (result!.Successful)
            {
                //===============Encode==================================
                var UTF8 = System.Text.Encoding.UTF8.GetBytes(result.Email!);
                var ToBase64String_Email = Convert.ToBase64String(UTF8);
                //=================================================

                NavigationManager!.NavigateTo($"/confirmemail/{ToBase64String_Email}");
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

