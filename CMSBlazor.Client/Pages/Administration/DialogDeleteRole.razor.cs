using System;
using CMSBlazor.Client.Service.Administration;
using CMSBlazor.Client.Service.Categories;
using CMSBlazor.Shared.ViewModels.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CMSBlazor.Client.Pages.Administration
{
	public partial class DialogDeleteRole
	{

        [Inject]
        public IAdministrationService? AdministrationService { get; set; }
        public RoleViewModel? RoleViewModel = new RoleViewModel();

        public bool ShowMessage;
        public string? Message;

        [Parameter]
        public string? roleId { get; set; }

        public async Task<string> HandleDelete()
        {
            ShowMessage = false;
            RoleViewModel!.Id = roleId;
           var result =  await AdministrationService!.DeleteRole(RoleViewModel);
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
           string message =  await HandleDelete();
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

