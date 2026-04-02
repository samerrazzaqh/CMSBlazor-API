using System;
using Blazored.LocalStorage;
using CMSBlazor.Client.Models;
using System.ComponentModel.Design;
using CMSBlazor.Client.Service.Comment;
using CMSBlazor.Client.Service.Posts;
using CMSBlazor.Shared.ViewModels.CommentLike;
using CMSBlazor.Shared.ViewModels.Post;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CMSBlazor.Client.Pages.Home
{
	public partial class DeleteCommentD
	{

        [Inject]
        public ICommentService? CommentService { get; set; }
        public CommentsViewModel? CommentsViewModel { get; set; } = new CommentsViewModel();

        public LocalStorageModel? localStorageModel { get; set; }


        [Inject]
        public ILocalStorageService? localStorageService { get; set; }

        [Parameter]
        public InfoComment? InfoComment { get; set; }

        public async Task HandleDelete()
        {
            localStorageModel = await localStorageService!.GetItemAsync<LocalStorageModel>("Storage");
            CommentsViewModel!.CommentId = InfoComment!.CommentId;
            CommentsViewModel!.PostId = InfoComment.PostId;
            CommentsViewModel.UserId = localStorageModel.UserId;
            await CommentService!.DeleteComment(CommentsViewModel!);
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

