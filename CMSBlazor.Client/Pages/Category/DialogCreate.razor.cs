using System;
using CMSBlazor.Client.Service.Categories;
using CMSBlazor.Shared.ViewModels.Post;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CMSBlazor.Client.Pages.Category
{
	public partial class DialogCreate
	{
        [Inject]
        public ICategoryService? categoryService { get; set; }
        public CategoryViewModel categoryViewModel = new CategoryViewModel();
        public bool ShowMessage;
        public string? Message;


        [Inject]
        public NavigationManager? NavigationManager { get; set; }


        public async Task HandleCreate()
        {
            ShowMessage = false;

             var result =  await categoryService!.CreateCategory(categoryViewModel);

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

