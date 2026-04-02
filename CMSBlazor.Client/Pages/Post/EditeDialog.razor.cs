using System;
using AutoMapper;
using CMSBlazor.Client.Helpers;
using CMSBlazor.Client.Service.Categories;
using CMSBlazor.Client.Service.Posts;
using CMSBlazor.Shared.ViewModels.Post;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace CMSBlazor.Client.Pages.Post
{
	public partial class EditeDialog
	{

        [Inject]
        public ICategoryService? categoryService { get; set; }
        public List<CategoryViewModel> Categories { get; set; } =
           new List<CategoryViewModel>();


        MudForm? form;
        [Inject]
        public IMapper? Mapper { get; set; }

        public PostUtilities? postUtilities { get; set; } = new PostUtilities();

        public PostViewModel? PostViewModel { get; set; } = new PostViewModel();

        public EditPostViewModel? EditPostViewModel { get; set; } = new EditPostViewModel();

        [Inject]
        public IPostService? PostService { get; set; }


        public bool ShowMessage;
        public bool Successful;
        public string? Message;

        [Parameter]
        public PostViewModel? post { get; set; }

        public string? pathImage,OldNameImage;
        bool changeImage = false;
        protected async override Task OnInitializedAsync()
        {
            postUtilities!.PostId = post!.PostId;

            EditPostViewModel!.CategoryId = post.CategoryId;
            Categories = (await categoryService!.GetCategories())!.ToList();


           
            PostViewModel = await PostService!.GetEditPost(postUtilities);
            OldNameImage = PostViewModel!.PostImg;
            pathImage =CMSConstant.BaseApiAddress+ $"/post/{PostViewModel!.PostImg}" ;
            Mapper!.Map(PostViewModel, EditPostViewModel);
           
            StateHasChanged();
        }


        //IList<IBrowserFile> files2 = new List<IBrowserFile>();
        public async Task UploadImageCover(IBrowserFile file)
        {
            //files2.Add(file);
            var buffer = new byte[file.Size];
            await file.OpenReadStream(long.MaxValue).ReadAsync(buffer);
            //convert byte array to base 64 string
            pathImage = $"data:image/png;base64,{Convert.ToBase64String(buffer)}";
            EditPostViewModel!.PostImg = Convert.ToBase64String(buffer);
            EditPostViewModel.PostImgName = file.Name;
            changeImage = true;
        }



        protected async Task HandleEdite()
        {
            ShowMessage = false;
            if (changeImage == false) {
                EditPostViewModel!.PostImg = null;
            }
            PostViewModel!.PostImgNameOld = OldNameImage;
            Mapper!.Map(EditPostViewModel, PostViewModel);
            
            var result = await PostService!.EditPost(PostViewModel!);

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

