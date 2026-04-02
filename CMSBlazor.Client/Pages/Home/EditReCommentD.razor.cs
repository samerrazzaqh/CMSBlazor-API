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
	public partial class EditReCommentD
	{

        [Inject]
        public IReCommentService? ReCommentService { get; set; }
        public ReComentsViewModel? ReComentsViewModel { get; set; } = new ReComentsViewModel();

        public LocalStorageModel? localStorageModel { get; set; }

        MudForm? form;
        [Inject]
        public ILocalStorageService? localStorageService { get; set; }

        [Parameter]
        public InfoReComment? InfoReComment { get; set; }

        public bool ShowMessage;
        public bool Successful;
        public string? Message;


        protected override Task OnInitializedAsync()
        {
            ReComentsViewModel!.ReTextComment = InfoReComment!.ReTextComment;

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
            else
            {

                localStorageModel = await localStorageService!.GetItemAsync<LocalStorageModel>("Storage");
                ReComentsViewModel!.CommentId = InfoReComment!.CommentId;
                ReComentsViewModel!.PostId = InfoReComment.PostId;
                ReComentsViewModel!.ReCommentId = InfoReComment.ReCommentId;
                ReComentsViewModel.UserId = localStorageModel.UserId;
                var result = await ReCommentService!.EditReComment(ReComentsViewModel!);

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

