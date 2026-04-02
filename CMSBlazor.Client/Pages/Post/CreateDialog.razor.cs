using System;
using Blazored.LocalStorage;
using CMSBlazor.Client.Models;
using CMSBlazor.Client.Service.Categories;
using CMSBlazor.Client.Service.Posts;
using CMSBlazor.Shared.ViewModels.Post;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;

namespace CMSBlazor.Client.Pages.Post
{
	public partial class CreateDialog
	{
        MudForm? form;
        [Inject]
        public ICategoryService? categoryService { get; set; }
        public List<CategoryViewModel> Categories { get; set; } =
           new List<CategoryViewModel>();


        [Inject]
        public IPostService? postService { get; set; }

        public PostViewModel postViewModel = new PostViewModel();
        public bool ShowMessage;
        public string? Message;
        
        public LocalStorageModel? localStorageModel { get; set; }
        [Inject]
        public ILocalStorageService? localStorageService { get; set; }

        public string? pathImage;
        bool addImage = false;


        protected override async Task OnInitializedAsync()
        {
            await  LoadData();
        }

        protected async Task LoadData()
        {
            postViewModel.CategoryId = 1;
            Categories = (await categoryService!.GetCategories())!.ToList();
        }

        //IList<IBrowserFile> files2 = new List<IBrowserFile>();
        public async Task UploadImageCover(IBrowserFile file)
        {
            //read that file in a byte array
            var buffer = new byte[file.Size];
            await file.OpenReadStream(long.MaxValue).ReadAsync(buffer);

            //convert byte array to base 64 string
            pathImage = $"data:image/png;base64,{Convert.ToBase64String(buffer)}";
            postViewModel.PostImg = Convert.ToBase64String(buffer);
            
            Console.WriteLine("+++++++++++++++    "+Convert.ToBase64String(buffer));
            postViewModel.PostImgName = file.Name;
            addImage = true;
        }


        public async Task HandleCreate()
        {
            await form!.Validate();
            if (form!.IsValid == false || addImage == false)
            {
                Message = "Fields must be filled";
                ShowMessage = true;
            }
            else {
                localStorageModel = await localStorageService!.GetItemAsync<LocalStorageModel>("Storage");
                postViewModel.Auther = localStorageModel.AboutUserId;
                var result = await postService!.CreatePost(postViewModel);

                if (result.Successful == true)
                {
                    Submit();
                }
                else
                {
                    Message = result.Message;
                    ShowMessage = true;
                }
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

