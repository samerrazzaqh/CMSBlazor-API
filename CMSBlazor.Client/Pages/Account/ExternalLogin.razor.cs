
using CMSBlazor.Client.Service.Account;
using CMSBlazor.Shared.ViewModels.Account;
using Microsoft.AspNetCore.Components;

namespace CMSBlazor.Client.Pages.Account
{
	public partial class ExternalLogin
	{
        [Parameter]
        [SupplyParameterFromQuery(Name = "email")]
        public string? Email { get; set; }

        //[Parameter]
        //[SupplyParameterFromQuery(Name = "firstname")]
        //public string? FirstName { get; set; }


        [Inject]
        public IAccountService? accountService { get; set; }
        public RegisterExternalViewModel registerExternal = new RegisterExternalViewModel();

        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        public bool ShowMessage;
        public bool Success;
        public string? Message;


        protected override async Task OnInitializedAsync()
        {
            if (Email == null) {
                Message = "Email not found";
                ShowMessage = true;
            }
            else {
                ShowMessage = false;

                //===============Decode==================================
                var FromBase64String = Convert.FromBase64String(Email!);
                var UTF8_Email = System.Text.Encoding.UTF8.GetString(FromBase64String);
                //=================================================

                registerExternal.Email = UTF8_Email;
                //registerExternal.FirstName = FirstName;
                var result = await accountService!.ExternalLogin(registerExternal);

                if (result!.Successful == true && result.Message == "SignedIn")
                {
                    NavigationManager!.NavigateTo("/");
                }
                else if (result!.Successful == false && result.Message == "Register")
                {
                    //===============Encode==================================
                    var UTF8 = System.Text.Encoding.UTF8.GetBytes(result.Email!);
                    var ToBase64String_Email = Convert.ToBase64String(UTF8);
                    //=================================================
                    NavigationManager!.NavigateTo($"/confirmemail/{ToBase64String_Email}");

                }
                else
                {
                    NavigationManager!.NavigateTo($"/login?MessageExternalLogin={result.Message}");
                    //Message = result.Message;
                    //ShowMessage = true;
                }
            }

           
            StateHasChanged();
        }



    }
}

