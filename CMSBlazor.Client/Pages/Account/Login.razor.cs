using System;
using CMSBlazor.Client.Service.Account;
using CMSBlazor.Shared.ViewModels.Account;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace CMSBlazor.Client.Pages.Account
{
    public partial class Login
	{
        [Parameter]
        [SupplyParameterFromQuery(Name = "MessageExternalLogin")]
        public string? MessageExternalLogin { get; set; }


        [Inject]
        public IAccountService? accountService { get; set; }
        public LoginViewModel loginModel = new LoginViewModel();
        public bool ShowMessage;
        public string? Message;
        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        public async Task HandleLogin()
        {
            ShowMessage = false;

            var result = await accountService!.Login(loginModel);

            if (result!.Successful)
            {
                NavigationManager!.NavigateTo("/");
            }
            else
            {
                Message = result.Message;
                ShowMessage = true;
            }
            StateHasChanged();
        }

        [Inject]
        public AuthenticationStateProvider? AuthenticationStateProvider { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if(MessageExternalLogin != null)
            {
                Message = MessageExternalLogin;
                ShowMessage = true;
            }
            var authState = await AuthenticationStateProvider!.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user.Identity!.IsAuthenticated)
            {
                //NavigationManager!.NavigateTo("javascript:history.back()");
                NavigationManager!.NavigateTo("/");
                StateHasChanged();
            }

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

