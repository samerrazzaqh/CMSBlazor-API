using System;
using Blazored.LocalStorage;
using CMSBlazor.Client.Helpers;
using CMSBlazor.Client.Models;
using CMSBlazor.Client.Service.Administration;
using CMSBlazor.Client.Service.Posts;
using CMSBlazor.Shared.ViewModels.Administration;
using CMSBlazor.Shared.ViewModels.Post;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;

namespace CMSBlazor.Client.Pages.Post
{
	public partial class ListPost
	{

        [Inject]
        public IPostService? PostService { get; set; }
        
        // [PersistentState]
        public PostAll? PostAll = new PostAll();
        public IEnumerable<PostViewModel> PostViewModels = new List<PostViewModel>();
        public LocalStorageModel? localStorageModel { get; set; }


        [Inject]
        public ILocalStorageService? localStorageService { get; set; }

        [Inject]
        public NavigationManager? NavigationManager { get; set; }


        //==================================================================
        public bool _policyCreate, _policyEdit, _policyDelete;
        public string? _SuperAdmin, _Admin, _User;
        [Inject]
        public IAdministrationService? AdministrationService { get; set; }
        public IEnumerable<Policy> Policies = new List<Policy>();
        public IEnumerable<Role> Roles = new List<Role>();
        public PolicyRole? PolicyClaims = new PolicyRole();
        public PolicyRole? PolicyRole = new PolicyRole();
        //=======================
        private async Task _Policy()
        {
            localStorageModel = await localStorageService!.GetItemAsync<LocalStorageModel>("Storage");

            PolicyClaims!.UserId = localStorageModel.UserId;
            PolicyClaims = await AdministrationService!.GetClaimsByUser(PolicyClaims!);
            Policies = PolicyClaims.policies!;

            PolicyRole!.UserId = localStorageModel.UserId;
            PolicyRole = await AdministrationService!.GetRolesByUser(PolicyRole!);
            Roles = PolicyRole.roles!;

            _policyCreate = Policies.Where(e => e.ClaimType == "Create Role").Select(e => e.ClaimValue).FirstOrDefault() == true ? false : true;
            _policyEdit = Policies.Where(e => e.ClaimType == "Edit Role").Select(e => e.ClaimValue).FirstOrDefault() == true ? false : true;
            _policyDelete = Policies.Where(e => e.ClaimType == "Delete Role").Select(e => e.ClaimValue).FirstOrDefault() == true ? false : true;

            _SuperAdmin = Roles.Where(e => e.RoleName == "SuperAdmin").Select(e => e.RoleName).FirstOrDefault();
            _Admin = Roles.Where(e => e.RoleName == "Admin").Select(e => e.RoleName).FirstOrDefault();
            _User = Roles.Where(e => e.RoleName == "User").Select(e => e.RoleName).FirstOrDefault();
        }
        //==================================================================
        public int num, PostsCount;
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
            num = 25;
            isVisible = true;
            await _Policy();
            PostAll = await PostService!.ListPost(num);
            PostsCount = PostAll.PostsCount;
            PostViewModels = PostAll.PostViewModels!;
            isVisible = false;
            StateHasChanged();
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
            num +=25;
            PostAll = await PostService!.ListPost(num);
            PostsCount = PostAll.PostsCount;
            PostViewModels = PostAll.PostViewModels!;
            isVisiblebtn = false;
            StateHasChanged();
        }

        public bool _sortNameByLength;
        public SortMode _sortMode = SortMode.Multiple;

        //// custom sort by name length
        private Func<PostViewModel, object> _sortBy => x =>
        {
            if (_sortNameByLength)
                return x.PostId;
            else
                return x.Title!;
        };


        public string? _searchString;
        public Func<PostViewModel, bool> _quickFilter => x =>
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;

            if (x.Title!.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        };




        [Inject]
        public IDialogService? DialogService { get; set; }
        DialogOptions maxWidth = new DialogOptions() { MaxWidth = MaxWidth.Large, FullWidth = true };

        public async Task OpenDialogCreate(DialogOptions options)
        {
            var dialog = await DialogService!.ShowAsync<CreateDialog>("Add New Post", options);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                PostAll = await PostService!.ListPost(num);
                PostsCount = PostAll.PostsCount;
                PostViewModels = PostAll.PostViewModels!;
                await SendMessage();
            }
        }

        public async Task OpenDialogEdite(DialogOptions options, PostViewModel model)
        {
            
            var parameters = new DialogParameters();
            parameters.Add("post", model);
            var dialog = await DialogService!.ShowAsync<EditeDialog>($"Edite", parameters, options);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                PostAll = await PostService!.ListPost(num);
                PostsCount = PostAll.PostsCount;
                PostViewModels = PostAll.PostViewModels!;
                await SendMessage();
            }
        }


        public async Task OpenDialogDelete(PostViewModel model)
        {
            var parameters = new DialogParameters();
            parameters.Add("postId", model.PostId);
            var dialog = await DialogService!.ShowAsync<DeleteDialog>($"Are you Delete {model.Title}", parameters);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                PostAll = await PostService!.ListPost(num);
                PostsCount = PostAll.PostsCount;
                PostViewModels = PostAll.PostViewModels!;
                await SendMessage();
            }
        }

        


    }
}

