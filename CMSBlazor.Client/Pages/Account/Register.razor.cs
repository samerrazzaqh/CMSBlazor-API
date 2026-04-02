using System;
using System.Reflection.Metadata;
using CMSBlazor.Client.Service.Account;
using CMSBlazor.Shared.ViewModels.Account;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CMSBlazor.Client.Pages.Account
{
    public partial class Register
	{
        [Inject]
        public IAccountService? accountService { get; set; }
        public RegisterViewModel RegisterModel = new RegisterViewModel();
        public bool ShowMessage;
        public bool Success;
        public string? Message;

        private bool isVisiblebtn;

        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        private async Task HandleRegistration()
        {
            isVisiblebtn = true;
            ShowMessage = false;

            var result = await accountService!.Register(RegisterModel);

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
                Success = result.Successful;
            }
            StateHasChanged();
            isVisiblebtn = false;
        }




        bool isShow;
        InputType PasswordInput = InputType.Password;
        string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
        public void ButtonTestclick()
        {
            if (isShow)
            {
                isShow = false;
                PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
                PasswordInput = InputType.Password;
            }
            else
            {
                isShow = true;
                PasswordInputIcon = Icons.Material.Filled.Visibility;
                PasswordInput = InputType.Text;
            }
        }











    }
}

