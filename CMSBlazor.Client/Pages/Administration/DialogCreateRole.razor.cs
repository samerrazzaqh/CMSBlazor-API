using System;
using CMSBlazor.Client.Service.Administration;
using CMSBlazor.Client.Service.Categories;
using CMSBlazor.Shared.ViewModels.Administration;
using CMSBlazor.Shared.ViewModels.Post;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CMSBlazor.Client.Pages.Administration
{
	public partial class DialogCreateRole
	{
        [Inject]
        public IAdministrationService? AdministrationService { get; set; }
        public CreateRoleViewModel CreateRoleViewModel = new CreateRoleViewModel();
        public bool ShowMessage;
        public string? Message;

        [Parameter]
        public EventCallback OnCreateCategory { get; set; }

        [Inject]
        public NavigationManager? NavigationManager { get; set; }


        public async Task HandleCreate()
        {
            ShowMessage = false;

            var result = await AdministrationService!.CreateRole(CreateRoleViewModel);

            if (result.Successful)
            {
                Submit();
            }
            else
            {
                Message = result.Message;
                ShowMessage = true;
            }
            StateHasChanged();
        }


        [CascadingParameter] IMudDialogInstance? MudDialog { get; set; }

        public void Submit()
        {
            MudDialog!.Close(DialogResult.Ok(true));

        }
        public void Cancel() => MudDialog!.Cancel();

    }
}

