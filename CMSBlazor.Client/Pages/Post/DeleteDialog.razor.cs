using System;
using CMSBlazor.Client.Service.Categories;
using CMSBlazor.Client.Service.Posts;
using CMSBlazor.Shared.ViewModels.Post;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CMSBlazor.Client.Pages.Post
{
	public partial class DeleteDialog
	{

        [Inject]
        public IPostService? PostService { get; set; }
        public PostUtilities postUtilities = new PostUtilities();


        [Parameter]
        public long postId { get; set; }

        public async Task HandleDelete()
        {
            postUtilities.PostId = postId;
            await PostService!.DeletePost(postUtilities);
        }


        [CascadingParameter] IMudDialogInstance? MudDialog { get; set; }

        public async Task Submit()
        {
            await HandleDelete();
            MudDialog!.Close(DialogResult.Ok(true));
            StateHasChanged();
        }
        public void Cancel() => MudDialog!.Cancel();
    }
}

