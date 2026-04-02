using System;
using CMSBlazor.Client.Service.Categories;
using CMSBlazor.Shared.ViewModels.Post;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CMSBlazor.Client.Pages.Category
{
	public partial class DialogDelete
	{

        [Inject]
        public ICategoryService? categoryService { get; set; }


        [Parameter]
        public int categoryId { get; set; }

        public async Task HandleDelete()
        {
            await categoryService!.DeleteCategory(categoryId);
        }


        [CascadingParameter] IMudDialogInstance? MudDialog { get; set; }

        public async Task  Submit()
        {
            await HandleDelete();
            MudDialog!.Close(DialogResult.Ok(true));
            StateHasChanged();
        }
        public void Cancel() => MudDialog!.Cancel();


    }
}

