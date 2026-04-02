using System;
using Blazored.LocalStorage;
using CMSBlazor.Client.Helpers;
using CMSBlazor.Client.Models;
using CMSBlazor.Client.Service.ReComment;
using CMSBlazor.Shared.Models;
using CMSBlazor.Shared.ViewModels.CommentLike;
using CMSBlazor.Shared.ViewModels.SignalR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;

namespace CMSBlazor.Client.Pages.Home
{
	public partial class ReComment
	{
        [Parameter]
        public InfoComment? InfoComment { get; set; }

        [Inject]
        public IReCommentService? ReCommentService { get; set; }
        public ReComentsViewModel? ReComentsViewModel { get; set; } = new ReComentsViewModel();
        public List<InfoReComment> InfoReComments { get; set; } = new List<InfoReComment>();


        public LocalStorageModel? localStorageModel { get; set; }
        [Inject]
        public ILocalStorageService? localStorageService { get; set; }

        private bool isVisiblebtn;
        private int loadmRcomment, numberRecomment;

        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        private HubConnection? hubConnection;

        protected override async Task OnInitializedAsync()
        {
            hubConnection = new HubConnectionBuilder()
           .WithUrl(NavigationManager!.ToAbsoluteUri($"{CMSConstant.BaseApiAddress}hubcontroller"))
            .Build();

            hubConnection.On("ReceiveMessage", async () =>
            {
                //if (PostlR.PostId == InfoComment!.PostId)
                //{
                //    await LoadData();
                //    await InvokeAsync(StateHasChanged);
                //}
                await LoadData();
                await InvokeAsync(StateHasChanged);
            });

            await hubConnection.StartAsync();
            await LoadData();
        }


        private async Task LoadData()
        {
            localStorageModel = await localStorageService!.GetItemAsync<LocalStorageModel>("Storage");
            await GetReComment();
            StateHasChanged();
        }

        //==================Hubs==================
        public bool IsConnected =>
               hubConnection?.State == HubConnectionState.Connected;
        public async Task SendMessage()
        {
            await hubConnection!.SendAsync("SendMessage");
        }

        public async ValueTask DisposeAsync()
        {
            if (hubConnection is not null)
            {
                await hubConnection.DisposeAsync();
            }
        }
        //==================End Hubs==================







        protected override async Task OnParametersSetAsync()
        {
            if (string.IsNullOrEmpty(string.Empty))
            {
                await GetReComment();
            }
            else
            {
                await GetReComment();
            }
            await base.OnParametersSetAsync();
        }

        protected async Task GetReComment()
        {
            loadmRcomment = 20;
            ReComentsViewModel!.PostId = InfoComment!.PostId;
            ReComentsViewModel!.CommentId = InfoComment!.CommentId;
            ReComentsViewModel.GetNumReComment = loadmRcomment;
            ReComentsViewModel = await ReCommentService!.ListReComment(ReComentsViewModel!);
            numberRecomment = ReComentsViewModel.numberRecomment;
            InfoReComments = ReComentsViewModel.InfoReComments!;
            StateHasChanged();
        }

        public async Task GetMoreReComments()
        {
            isVisiblebtn = true;
            loadmRcomment += 3;
            ReComentsViewModel!.PostId = InfoComment!.PostId;
            ReComentsViewModel!.CommentId = InfoComment!.CommentId;
            ReComentsViewModel.GetNumReComment = loadmRcomment;
            ReComentsViewModel = await ReCommentService!.ListReComment(ReComentsViewModel!);
            InfoReComments = ReComentsViewModel.InfoReComments!;
            numberRecomment = ReComentsViewModel.numberRecomment;
            StateHasChanged();
            isVisiblebtn = false;
        }

        private string? TextReComment { get; set; }
        public async Task CreateReComment()
        {
            ReComentsViewModel!.ReTextComment = TextReComment;
            ReComentsViewModel!.PostId = InfoComment!.PostId;
            ReComentsViewModel!.CommentId = InfoComment!.CommentId;
            ReComentsViewModel.UserId = localStorageModel!.UserId;
            var result = await ReCommentService!.CreateReComment(ReComentsViewModel!);
            if (result.Successful == true)
            {
                await GetReComment();
                TextReComment = "";
                await SendMessage();
            }
            StateHasChanged();
        }


      




        [Inject]
        public IDialogService? DialogService { get; set; }
        DialogOptions maxWidth = new DialogOptions() { MaxWidth = MaxWidth.Large, FullWidth = true };

        public async Task EditTextReComment(DialogOptions options, InfoReComment infoReComment)
        {
            var parameters = new DialogParameters();
            parameters.Add("InfoReComment", infoReComment);
            var dialog = await DialogService!.ShowAsync<EditReCommentD>($"Edit this Comment", parameters, options);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                await GetReComment();
                await SendMessage();
            }
        }





        public async Task DeleteReComment(InfoReComment infoReComment)
        {

            var parameters = new DialogParameters();
            parameters.Add("InfoReComment", infoReComment);
            var dialog = await DialogService!.ShowAsync<DeleteReCommentD>($"Delete this Comment", parameters);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
               await GetReComment();
               await SendMessage();
            }
        }











    }
}

