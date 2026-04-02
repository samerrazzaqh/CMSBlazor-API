using System;
using System.Xml.Linq;
using Blazored.LocalStorage;
using CMSBlazor.Client.Models;
using CMSBlazor.Client.Service.Administration;
using CMSBlazor.Client.Service.Categories;
using CMSBlazor.Shared.ViewModels.Administration;
using CMSBlazor.Shared.ViewModels.Post;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CMSBlazor.Client.Pages.Category
{
	public partial class ListCategory
	{
        [Inject]
        public ICategoryService? CategoryService { get; set; }
        //public IEnumerable<CategoryViewModel>? CategoryViewModels { get; set; }
        public IEnumerable<CategoryViewModel> CategoryViewModels = new List<CategoryViewModel>();
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


        private bool isVisible;
        protected override async Task OnInitializedAsync()
        {
            isVisible = true;
            await _Policy();
            CategoryViewModels = (await CategoryService!.GetCategories())!.ToList();
            StateHasChanged();
            isVisible = false;

        }

        public bool _sortNameByLength;
        public SortMode _sortMode = SortMode.Multiple;

        //// custom sort by name length
        private Func<CategoryViewModel, object> _sortBy => x =>
        {
            if (_sortNameByLength)
                return x.CategoryId;
            else
                return x.CatName!;
        };


        public string? _searchString;
        public Func<CategoryViewModel, bool> _quickFilter => x =>
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;

            if (x.CatName!.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        };




        [Inject]
        public IDialogService? DialogService { get; set; }
        DialogOptions maxWidth = new DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true };

        public async Task OpenDialogCreate(DialogOptions options)
        {
            var dialog = await DialogService!.ShowAsync<DialogCreate>("Add New Category", options);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                CategoryViewModels = (await CategoryService!.GetCategories())!.ToList();
            }
        }

        public async Task OpenDialogEdite(DialogOptions options,CategoryViewModel model)
        {
            var parameters = new DialogParameters();
            parameters.Add("category", model);
            var dialog = await DialogService!.ShowAsync<DialogEdite>($"Edite", parameters,options);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                CategoryViewModels = (await CategoryService!.GetCategories())!.ToList();
            }


        }


        public async Task OpenDialogDelete(CategoryViewModel model)
        {
            var parameters = new DialogParameters();
            parameters.Add("categoryId", model.CategoryId);
            var dialog = await DialogService!.ShowAsync<DialogDelete>($"Are you Delete {model.CatName}", parameters);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                CategoryViewModels = (await CategoryService!.GetCategories())!.ToList();
            }
        }



    }
}

