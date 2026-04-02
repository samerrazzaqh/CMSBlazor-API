using System;
using Blazored.LocalStorage;
using CMSBlazor.Client.Helpers;
using CMSBlazor.Client.Models;
using CMSBlazor.Client.Pages.Category;
using CMSBlazor.Client.Service.Categories;
using CMSBlazor.Client.Service.Home;
using CMSBlazor.Client.Service.Posts;
using CMSBlazor.Shared.ViewModels.Post;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;

namespace CMSBlazor.Client.Pages.Home
{
	public partial class Index
	{
        [Inject]
        public IHomeService? HomeService { get; set; }

        public IEnumerable<PostViewModel> PostViewModels = new List<PostViewModel>();
        public LocalStorageModel? localStorageModel { get; set; }


        [Inject]
        public ILocalStorageService? localStorageService { get; set; }

        [Inject]
        public NavigationManager? NavigationManager { get; set; }


        //==================================================================
        public int num;
        private bool isVisible, isVisiblebtn;
        private HubConnection? hubConnection;

        protected override async Task OnInitializedAsync()
        {
            hubConnection = new HubConnectionBuilder()
          .WithUrl(NavigationManager!.ToAbsoluteUri($"{CMSConstant.BaseApiAddress}hubcontroller"))
            .Build();

            hubConnection.On("ReceiveMessage", async () =>
            {
                await LoadData();
                StateHasChanged();
            });

            await hubConnection.StartAsync();
            await LoadData();
        }


        private async Task LoadData()
        {
            isVisible = true;
            await Carousel();
            await Storis();
            await Category();
            await MostPopularPostList();
            await LatestVideosList();
            StateHasChanged();
            isVisible = false;
        }

        //==================Hubs==================
        public bool IsConnected =>
               hubConnection?.State == HubConnectionState.Connected;
        Task SendMessage() => hubConnection!.SendAsync("SendMessage");

        public async ValueTask DisposeAsync()
        {
            if (hubConnection is not null)
            {
                await hubConnection.DisposeAsync();
            }
        }
        //==================End Hubs==================


        public IEnumerable<PostViewModel> StoryModel = new List<PostViewModel>();
        protected async Task Storis()
        {
            num = 30;
            StoryModel = (await HomeService!.ListPost(num))!.ToList();
            StateHasChanged();
        }

        public IEnumerable<PostViewModel> CarouselModel = new List<PostViewModel>();
        protected async Task Carousel()
        {
            CarouselModel = (await HomeService!.ListPost(3))!.ToList();
            StateHasChanged();
        }

        public IEnumerable<PostViewModel> LatestVideos = new List<PostViewModel>();
        protected async Task LatestVideosList()
        {
            LatestVideos = (await HomeService!.ListPost(4))!.ToList();
            StateHasChanged();
        }


        public IEnumerable<PostViewModel> MostPopularPost = new List<PostViewModel>();
        protected async Task MostPopularPostList()
        {
            MostPopularPost = (await HomeService!.MostPopularPost(4))!.ToList();
            StateHasChanged();
        }

        [Inject]
        public ICategoryService? CategoryService { get; set; }
        public IEnumerable<CategoryViewModel> CategoryViewModels = new List<CategoryViewModel>();
        protected async Task Category()
        {
            CategoryViewModels = (await CategoryService!.GetCategories())!.ToList();
            StateHasChanged();
        }

        public async Task GetMorePost()
        {
            isVisiblebtn = true;
            num += 30;
            StoryModel = (await HomeService!.ListPost(num))!.ToList();
            isVisiblebtn = false;
            StateHasChanged();
        }

        public void ListPostByCategory(int CategoryId,string CatName)
        {
            NavigationManager!.NavigateTo($"/listpostbycategory/{CategoryId}/{CatName}");
        }

        public void ReadMore(long PostId)
        {
            NavigationManager!.NavigateTo($"/singlepost/{PostId}");
        }



    }
}

