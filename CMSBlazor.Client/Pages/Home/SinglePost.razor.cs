using System;
using System.ComponentModel;
using Blazored.LocalStorage;
using CMSBlazor.Client.Helpers;
using CMSBlazor.Client.Models;
using CMSBlazor.Client.Service.Comment;
using CMSBlazor.Client.Service.Home;
using CMSBlazor.Shared.ViewModels.Administration;
using CMSBlazor.Shared.ViewModels.CommentLike;
using CMSBlazor.Shared.ViewModels.Post;
using CMSBlazor.Shared.ViewModels.SignalR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;

namespace CMSBlazor.Client.Pages.Home
{
	public partial class SinglePost
	{
        [Inject]
        public IHomeService? HomeService { get; set; }
        public PostViewModel? PostViewModel { get; set; } = new PostViewModel();
        public IEnumerable<UsersLikePost> UsersLikePosts = new List<UsersLikePost>();
        public PostUtilities? postUtilities { get; set; } = new PostUtilities();

   




        public LocalStorageModel? localStorageModel { get; set; }


        [Inject]
        public ILocalStorageService? localStorageService { get; set; }

        [Inject]
        public NavigationManager? NavigationManager { get; set; }


        //==================================================================
        [Parameter]
        public long postId { get; set; }
        public bool isLiketoggled { get; set; }
        public int num;
        private bool isVisible, isVisiblebtn;


        private HubConnection? hubConnection;

        protected override async Task OnInitializedAsync()
        {
            isVisible = true;
            localStorageModel = await localStorageService!.GetItemAsync<LocalStorageModel>("Storage");
            hubConnection = new HubConnectionBuilder()
           .WithUrl(NavigationManager!.ToAbsoluteUri($"{CMSConstant.BaseApiAddress}hubcontroller"))
             .Build();

            hubConnection.On("ReceiveMessage", async () =>
            {
                //if(PostlR.PostId == postId)
                //{
                //    await LoadData();
                //    await InvokeAsync(StateHasChanged);
                //}
                await LoadData();
                await InvokeAsync(StateHasChanged);
            });

            await hubConnection.StartAsync();
            await LoadData();
            isVisible = false;
        }


        private async Task LoadData()
        {
            localStorageModel = await localStorageService!.GetItemAsync<LocalStorageModel>("Storage");
            await GetPost();
            await GetComments();
            await InvokeAsync(StateHasChanged);
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



        protected async Task GetPost()
        {
            postUtilities!.PostId = postId;
            PostViewModel = await HomeService!.SinglePost(postUtilities!);
            UsersLikePosts = PostViewModel.UsersLikePosts!;
            StateHasChanged();
        }

        public async Task LikePostCreate()
        {
            CommentsViewModel!.PostId = postId;
            CommentsViewModel.UserId = localStorageModel!.UserId;
            PostViewModel = await CommentService!.LikePostCreate(CommentsViewModel!);
            UsersLikePosts = PostViewModel.UsersLikePosts!;
            await SendMessage();
            await InvokeAsync(StateHasChanged);
        }

        //========================Comments ==========================================
        [Inject]
        public ICommentService? CommentService { get; set; }
        public CommentsViewModel? CommentsViewModel { get; set; } = new CommentsViewModel();
        public IEnumerable<InfoComment> InfoComments = new List<InfoComment>();
        public LikesComment LikesComments { get; set; } = new LikesComment();

        public bool LikeComment, LikePost;
        public string? FavoriteBorder = Icons.Material.Filled.FavoriteBorder;
        public string? Favorite = Icons.Material.Filled.Favorite;

        [Inject]
        public IDialogService? DialogService { get; set; }
        DialogOptions maxWidth = new DialogOptions() { MaxWidth = MaxWidth.Large, FullWidth = true };

        protected async Task GetComments()
        {
            num = 4;
            CommentsViewModel!.PostId = postId;
            CommentsViewModel.GetNumComment = num;
            CommentsViewModel = await CommentService!.ListComment(CommentsViewModel!);
            InfoComments = CommentsViewModel.InfoComments!;

            StateHasChanged();
        }


        public async Task GetMoreComments()
        {
            isVisiblebtn = true;
            num += 5; 
            CommentsViewModel!.PostId = postId;
            CommentsViewModel.GetNumComment = num;
            CommentsViewModel = await CommentService!.ListComment(CommentsViewModel!);
            InfoComments = CommentsViewModel.InfoComments!;
            StateHasChanged();
            isVisiblebtn = false;
        }



        private string? TextComment { get; set; }
        public async Task CreateComment()
        {
            localStorageModel = await localStorageService!.GetItemAsync<LocalStorageModel>("Storage");
            CommentsViewModel!.TextComment = TextComment;
            CommentsViewModel!.PostId = postId;
            CommentsViewModel.UserId = localStorageModel.UserId;
            var result = await CommentService!.CreateComment(CommentsViewModel!);
            if (result.Successful == true) {
                CommentsViewModel!.PostId = postId;
                CommentsViewModel.GetNumComment = num;
                CommentsViewModel = await CommentService!.ListComment(CommentsViewModel!);
                InfoComments = CommentsViewModel.InfoComments!;
                TextComment = "";
                await SendMessage();
            }
            StateHasChanged();
        }

        

        public LikesComment? LikesComment { get; set; } = new LikesComment();
        public async Task LikeComments(InfoComment infoComment)
        {
            LikesComment!.CommentId = infoComment.CommentId;
            LikesComment!.PostId = postId;
            LikesComment!.UserId = localStorageModel!.UserId;
            var result = await CommentService!.LikeCommentCreate(LikesComment!);
            if (result.Successful == true)
            {
                CommentsViewModel!.PostId = postId;
                CommentsViewModel.GetNumComment = num;
                CommentsViewModel = await CommentService!.ListComment(CommentsViewModel!);
                InfoComments = CommentsViewModel.InfoComments!;
                TextComment = "";
                await SendMessage();
            }
            StateHasChanged();
        }

        public async Task EditTextComment(DialogOptions options, InfoComment infoComment)
        {
            var parameters = new DialogParameters();
            parameters.Add("InfoComment", infoComment);
            var dialog = await DialogService!.ShowAsync<EditCommentD>($"Edit this Comment", parameters, options);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                CommentsViewModel!.PostId = postId;
                CommentsViewModel.GetNumComment = num;
                CommentsViewModel = await CommentService!.ListComment(CommentsViewModel!);
                InfoComments = CommentsViewModel.InfoComments!;
                await SendMessage();
            }
        }
        public async Task DeleteComment(InfoComment infoComment)
        {

            var parameters = new DialogParameters();
            parameters.Add("InfoComment", infoComment);
            var dialog = await DialogService!.ShowAsync<DeleteCommentD>($"Delete this Comment", parameters);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                CommentsViewModel!.PostId = postId;
                CommentsViewModel.GetNumComment = num;
                CommentsViewModel = await CommentService!.ListComment(CommentsViewModel!);
                InfoComments = CommentsViewModel.InfoComments!;
                await SendMessage();
            }
        }
        













    }
}

