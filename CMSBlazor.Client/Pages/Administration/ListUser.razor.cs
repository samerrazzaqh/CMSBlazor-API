using System;
using Blazored.LocalStorage;
using CMSBlazor.Client.Models;
using CMSBlazor.Client.Service.Administration;
using CMSBlazor.Shared.ViewModels.Administration;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CMSBlazor.Client.Pages.Administration
{
	public partial class ListUser
	{
        public IEnumerable<Users> Users = new List<Users>();
        

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
        private async Task _Policy() {
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
            Users = (await AdministrationService!.ListUsers()).ToList();
            isVisible = false;
            StateHasChanged();

        }

        public bool _sortNameByLength;
        public SortMode _sortMode = SortMode.Multiple;

        //// custom sort by name length
        private Func<Users, object> _sortBy => x =>
        {
            if (_sortNameByLength)
                return x.Id!;
            else
                return x.UserName!;
        };


        public string? _searchString;
        public Func<Users, bool> _quickFilter => x =>
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;

            if (x.UserName!.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        };




        [Inject]
        public IDialogService? DialogService { get; set; }
        //DialogOptions maxWidth = new DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true };
        public void OpenDialogEdite(Users model)
        {
            NavigationManager!.NavigateTo($"/edituser/{model.Id}");
        }


        public async Task OpenDialogDelete(Users model)
        {
            var parameters = new DialogParameters();
            parameters.Add("userId", model.Id);
            var dialog = await DialogService!.ShowAsync<DialogDeleteUser>($"Are you Delete {model.UserName}", parameters);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                Users = (await AdministrationService!.ListUsers()).ToList();
                StateHasChanged();
            }
        }






    }
}

