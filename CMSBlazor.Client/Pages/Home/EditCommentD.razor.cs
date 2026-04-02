using System;
using AutoMapper;
using Blazored.LocalStorage;
using CMSBlazor.Client.Models;
using CMSBlazor.Client.Service.Categories;
using CMSBlazor.Client.Service.Comment;
using CMSBlazor.Client.Service.Posts;
using CMSBlazor.Shared.Models;
using CMSBlazor.Shared.ViewModels.CommentLike;
using CMSBlazor.Shared.ViewModels.Post;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CMSBlazor.Client.Pages.Home
{
	public partial class EditCommentD
	{
        [Inject]
        public ICommentService? CommentService { get; set; }
        public CommentsViewModel? CommentsViewModel { get; set; } = new CommentsViewModel();

        public LocalStorageModel? localStorageModel { get; set; }

        MudForm? form;
        [Inject]
        public ILocalStorageService? localStorageService { get; set; }

        [Parameter]
        public InfoComment? InfoComment { get; set; }

        public bool ShowMessage;
        public bool Successful;
        public string? Message;


        protected override Task OnInitializedAsync()
        {
            CommentsViewModel!.TextComment = InfoComment!.TextComment;

            StateHasChanged();
            return Task.CompletedTask;
        }

        protected async Task HandleEdite()
        {
            ShowMessage = false;
            await form!.Validate();
            if (form!.IsValid == false)
            {
                ShowMessage = true;
                Message = "Fields must be filled";
            }
            else {

                localStorageModel = await localStorageService!.GetItemAsync<LocalStorageModel>("Storage");
                CommentsViewModel!.CommentId = InfoComment!.CommentId;
                CommentsViewModel!.PostId = InfoComment.PostId;
                CommentsViewModel.UserId = localStorageModel.UserId;
                var result = await CommentService!.EditComment(CommentsViewModel!);

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

            

        }



        [CascadingParameter] IMudDialogInstance? MudDialog { get; set; }

        public void Submit()
        {
            MudDialog!.Close(DialogResult.Ok(true));

        }

        public void Cancel() => MudDialog!.Cancel();


    }
}

