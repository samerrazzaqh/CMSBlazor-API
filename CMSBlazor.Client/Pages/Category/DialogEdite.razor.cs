using System;
using AutoMapper;
using CMSBlazor.Client.Service.Categories;
using CMSBlazor.Client.Service.Profile;
using CMSBlazor.Shared.ViewModels.Post;
using CMSBlazor.Shared.ViewModels.Profile;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CMSBlazor.Client.Pages.Category
{
	public partial class DialogEdite
	{
        [Inject]
        public IMapper? Mapper { get; set; }

        public CategoryViewModel? CategoryViewModel { get; set; } = new CategoryViewModel();

        public EditeCategoryModel? EditeCategoryModel { get; set; } = new EditeCategoryModel();

        [Inject]
        public ICategoryService? CategoryService { get; set; }


        public bool ShowMessage;
        public bool Successful;
        public string? Message;

        [Parameter]
        public CategoryViewModel? category { get; set; }


        protected async override Task OnInitializedAsync()
        {
            CategoryViewModel = await CategoryService!.GetCategory(category!.CategoryId);
            Mapper!.Map(CategoryViewModel, EditeCategoryModel);
        }




        protected async Task HandleEdite()
        {
            ShowMessage = false;
            Mapper!.Map(EditeCategoryModel, CategoryViewModel);
           var result = await CategoryService!.UpdateCategory(CategoryViewModel!);

            if (result!.Successful)
            {
                Submit();
            }
            else
            {
                ShowMessage = true;
                Message = result.Message;
            }

        }


        [CascadingParameter] IMudDialogInstance? MudDialog { get; set; }

        public void Submit()
        {
            MudDialog!.Close(DialogResult.Ok(true));

        }

        public void Cancel() => MudDialog!.Cancel();



    }
}

