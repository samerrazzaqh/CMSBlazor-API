using System;
using AutoMapper;
using Blazored.LocalStorage;
using CMSBlazor.Client.Models;
using CMSBlazor.Client.Service.Administration;
using CMSBlazor.Shared.ViewModels.Administration;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CMSBlazor.Client.Pages.Administration
{
	public partial class EditUser
	{

        [Parameter]
        public string? Id { get; set; }

        [Inject]
        public IMapper? Mapper { get; set; }

        public UserID? UserID = new UserID();

        public UserViewModel? UserViewModel = new UserViewModel();
        public EditUserViewModel? EditUserViewModel = new EditUserViewModel();


        public bool ShowMessage;
        public string? Message;
        [Inject]
        public NavigationManager? NavigationManager { get; set; }


        //==================================================================
        public LocalStorageModel? localStorageModel { get; set; }
        [Inject]
        public ILocalStorageService? localStorageService { get; set; }
        public bool _policyCreate, _policyEdit, _policyDelete;
        public string? _SuperAdmin, _Admin, _User;
        [Inject]
        public IAdministrationService? AdministrationService { get; set; }
        public IEnumerable<Policy> Policies = new List<Policy>();
        public IEnumerable<Role> Role = new List<Role>();
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
            Role = PolicyRole.roles!;

            _policyEdit = Policies.Where(e => e.ClaimType == "Edit Role").Select(e => e.ClaimValue).FirstOrDefault() == true ? true : false;
            _SuperAdmin = Role.Where(e => e.RoleName == "SuperAdmin").Select(e => e.RoleName).FirstOrDefault();
            _Admin = Role.Where(e => e.RoleName == "Admin").Select(e => e.RoleName).FirstOrDefault();
        }
        //==================================================================



        protected override async Task OnInitializedAsync()
        {
            await _Policy();
            UserID!.UserId = Id;
            UserViewModel = await AdministrationService!.GetEditUser(UserID);
            Mapper!.Map(UserViewModel, EditUserViewModel);
        }



        public async Task HandleEditUser()
        {
            ShowMessage = false;
            Mapper!.Map(EditUserViewModel, UserViewModel);
            var result = await AdministrationService!.EditUser(UserViewModel!);

            if (result!.Successful)
            {
                StateHasChanged();
            }
        }



        [Inject]
        public IDialogService? DialogService { get; set; }
        DialogOptions maxWidth = new DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true };

        public async Task AddorRemoveRole(DialogOptions options, EditUserViewModel model)
        {
            var parameters = new DialogParameters();
            parameters.Add("userId", model.Id);
            var dialog = await DialogService!.ShowAsync<ManageUserRoles>($"Add or Remove", parameters, options);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                UserID!.UserId = Id;
                UserViewModel = await AdministrationService!.GetEditUser(UserID);
                Mapper!.Map(UserViewModel, EditUserViewModel);
            }
        }


        public async Task AddorRemoveClaim(DialogOptions options, EditUserViewModel model)
        {
            var parameters = new DialogParameters();
            parameters.Add("userId", model.Id);
            var dialog = await DialogService!.ShowAsync<ManageUserClaims>($"Add or Remove", parameters, options);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                UserID!.UserId = Id;
                UserViewModel = await AdministrationService!.GetEditUser(UserID);
                Mapper!.Map(UserViewModel, EditUserViewModel);
            }
        }




    }
}

