using System;
using Blazored.LocalStorage;
using CMSBlazor.Client.Models;
using CMSBlazor.Client.Service.Comment;
using CMSBlazor.Client.Service.ReComment;
using CMSBlazor.Shared.ViewModels.CommentLike;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CMSBlazor.Client.Pages.Home
{
	public partial class DeleteReCommentD
	{
        [Inject]
        public IReCommentService? ReCommentService { get; set; }
        public ReComentsViewModel? ReComentsViewModel { get; set; } = new ReComentsViewModel();

        public LocalStorageModel? localStorageModel { get; set; }


        [Inject]
        public ILocalStorageService? localStorageService { get; set; }

        [Parameter]
        public InfoReComment? InfoReComment { get; set; }

        public async Task HandleDelete()
        {
            localStorageModel = await localStorageService!.GetItemAsync<LocalStorageModel>("Storage");
            ReComentsViewModel!.CommentId = InfoReComment!.CommentId;
            ReComentsViewModel!.PostId = InfoReComment.PostId;
            ReComentsViewModel!.ReCommentId = InfoReComment.ReCommentId;
            ReComentsViewModel.UserId = localStorageModel.UserId;
            await ReCommentService!.DeleteReComment(ReComentsViewModel!);
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

