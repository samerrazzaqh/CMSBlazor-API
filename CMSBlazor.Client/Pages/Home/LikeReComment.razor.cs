using System;
using Blazored.LocalStorage;
using CMSBlazor.Client.Helpers;
using CMSBlazor.Client.Models;
using CMSBlazor.Client.Service.ReComment;
using CMSBlazor.Shared.ViewModels.CommentLike;
using CMSBlazor.Shared.ViewModels.SignalR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;

namespace CMSBlazor.Client.Pages.Home
{
	public partial class LikeReComment
	{

        [Parameter]
        public InfoReComment? InfoReComments { get; set; }

        [Inject]
        public IReCommentService? ReCommentService { get; set; }
        public ReComentsViewModel? ReComentsViewModel { get; set; } = new ReComentsViewModel();
        private List<LikeReComments> LikeReComments { get; set; } = new List<LikeReComments>();


        public LocalStorageModel? localStorageModel { get; set; }
        [Inject]
        public ILocalStorageService? localStorageService { get; set; }

        public string? FavoriteBorder = Icons.Material.Filled.FavoriteBorder;
        public string? Favorite = Icons.Material.Filled.Favorite;
        public int numUserLike;


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
                //if (PostlR.PostId == InfoReComments!.PostId)
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
            numUserLike = 1;
            await GetLikeReComment();
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
                await GetLikeReComment();
            }
            else
            {
                await GetLikeReComment();
            }
           await base.OnParametersSetAsync();
        }


        protected async Task GetLikeReComment()
        {

            ReComentsViewModel!.PostId = InfoReComments!.PostId;
            ReComentsViewModel!.CommentId = InfoReComments.CommentId;
            ReComentsViewModel!.ReCommentId = InfoReComments.ReCommentId;

            ReComentsViewModel = await ReCommentService!.ListLikeReComment(ReComentsViewModel);
            LikeReComments = ReComentsViewModel.LikeReComments!;
            StateHasChanged();
        }


       
        public async Task AddLikesReComment(InfoReComment model)
        {
            ReComentsViewModel!.CommentId = model.CommentId;
            ReComentsViewModel!.PostId = model.PostId;
            ReComentsViewModel!.ReCommentId = model.ReCommentId;
            ReComentsViewModel!.UserId = localStorageModel!.UserId;
            var result = await ReCommentService!.LikeReCommentCreate(ReComentsViewModel!);
            if (result.Successful == true)
            {
               await GetLikeReComment();
               await SendMessage();
            }
            StateHasChanged();
        }

      


    }
}

