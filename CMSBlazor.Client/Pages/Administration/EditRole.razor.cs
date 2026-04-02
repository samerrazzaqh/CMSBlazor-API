using System;
using AutoMapper;
using Blazored.LocalStorage;
using CMSBlazor.Client.Models;
using CMSBlazor.Client.Pages.Category;
using CMSBlazor.Client.Service.Administration;
using CMSBlazor.Client.Service.Categories;
using CMSBlazor.Shared.ViewModels.Administration;
using CMSBlazor.Shared.ViewModels.Post;
using CMSBlazor.Shared.ViewModels.Profile;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CMSBlazor.Client.Pages.Administration
{
	public partial class EditRole
	{
        [Parameter]
        public string? Id { get; set; }

        [Inject]
        public IMapper? Mapper { get; set; }
        public RoleViewModel? RoleViewModel = new RoleViewModel();

        public EditRoleViewModel? EditRoleViewModel = new EditRoleViewModel();
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
            RoleViewModel = await AdministrationService!.GetEditRole(Id!);
            Mapper!.Map(RoleViewModel, EditRoleViewModel);
        }





        public async Task HandleEditRole()
        {
            ShowMessage = false;
            Mapper!.Map(EditRoleViewModel, RoleViewModel);
            var result = await AdministrationService!.EditRole(RoleViewModel!);

            if (result!.Successful)
            {
                StateHasChanged();
            }
            else
            {
                Message = result.Message;
                ShowMessage = true;
                RoleViewModel = await AdministrationService!.GetEditRole(Id!);
                Mapper!.Map(RoleViewModel, EditRoleViewModel);
                StateHasChanged();
            }
            StateHasChanged();
        }






        [Inject]
        public IDialogService? DialogService { get; set; }
        DialogOptions maxWidth = new DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true };

        public async Task AddorRemoveUser(DialogOptions options, EditRoleViewModel model)
        {
            var parameters = new DialogParameters();
            parameters.Add("roleId", model.Id);
            var dialog = await DialogService!.ShowAsync<EditUsersInRole>($"Add or Remove", parameters, options);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                RoleViewModel = await AdministrationService!.GetEditRole(Id!);
                Mapper!.Map(RoleViewModel, EditRoleViewModel);
            }
        }




    }
}

