using System;
using CMSBlazor.Client.Helpers;
using CMSBlazor.Client.Service.Categories;
using CMSBlazor.Client.Service.Home;
using CMSBlazor.Shared.ViewModels.Post;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace CMSBlazor.Client.Pages.Home
{
	public partial class ListPostByCategory
	{
        [Parameter]
        public int categoryId { get; set; }

        [Parameter]
        public string? categoryName { get; set; }

        [Inject]
        public IHomeService? HomeService { get; set; }

        [Inject]
        public NavigationManager? NavigationManager { get; set; }


        public string? CatName;
        public int num, CatId;
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
            await ListPostCat();
            await Category();
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



        public async Task GetMorePost()
        {
            isVisiblebtn = true;
            num += 25;
            CatName = categoryName;
            CatId = categoryId!;
            PostViewModels = (await HomeService!.ListPostByCategory(num, CatId))!.ToList();
            StateHasChanged();
            isVisiblebtn = false;
        }

        public IEnumerable<PostViewModel> PostViewModels = new List<PostViewModel>();
        protected async Task ListPostCat()
        {
            num = 25;
            CatName = categoryName;
            CatId = categoryId!;
            PostViewModels = (await HomeService!.ListPostByCategory(num, CatId))!.ToList();
            StateHasChanged();
        }

        [Inject]
        public ICategoryService? CategoryService { get; set; }
        public IEnumerable<CategoryViewModel> CategoryViewModels = new List<CategoryViewModel>();
        protected async Task Category()
        {
            num = 16;
            CategoryViewModels = (await CategoryService!.GetCategories())!.ToList();
            StateHasChanged();
        }

        public void GoListPostByCategory(int CategoryId, string CatName)
        {
            NavigationManager!.NavigateTo("/");
            NavigationManager!.NavigateTo($"/listpostbycategory/{CategoryId}/{CatName}");
        }


        public void ReadMore(long PostId)
        {
            NavigationManager!.NavigateTo($"/singlepost/{PostId}");
        }


    }
}

