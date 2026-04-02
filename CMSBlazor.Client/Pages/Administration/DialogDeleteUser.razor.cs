using System;
using CMSBlazor.Client.Service.Administration;
using CMSBlazor.Shared.ViewModels.Administration;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CMSBlazor.Client.Pages.Administration
{
	public partial class DialogDeleteUser
	{
        [Inject]
        public IAdministrationService? AdministrationService { get; set; }
        public UserID? UserID = new UserID();

        public bool ShowMessage;
        public string? Message;



        [Parameter]
        public string? userId { get; set; }

        public async Task<string> HandleDelete()
        {
            ShowMessage = false;
            UserID!.UserId = userId;
            var result = await AdministrationService!.DeleteUser(UserID);
            if (result.Successful == false)
            {

                return result.Message!;
            }
            else
            {
                return null!;
            }
        }


        [CascadingParameter] IMudDialogInstance? MudDialog { get; set; }

        public async Task Submit()
        {
            string message = await HandleDelete();
            if (message != null)
            {
                ShowMessage = true;
                Message = message;
            }
            else
            {
                MudDialog!.Close(DialogResult.Ok(true));
                StateHasChanged();
            }

        }
        public void Cancel() => MudDialog!.Cancel();



    }
}

