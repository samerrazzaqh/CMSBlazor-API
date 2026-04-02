using System;
using AutoMapper;
using CMSBlazor.Client.Service.Administration;
using CMSBlazor.Shared.ViewModels.Administration;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CMSBlazor.Client.Pages.Administration
{
	public partial class ManageUserRoles
	{

        [Inject]
        public IMapper? Mapper { get; set; }

        [Inject]
        public IAdministrationService? AdministrationService { get; set; }
        public UserID? UserID = new UserID();
        public UserRoleViewModel? UserRoleViewModel = new UserRoleViewModel();

        public List<RolesUser> RolesUsers = new List<RolesUser>();


        public EditUserRoleViewModel? EditUserRoleViewModel = new EditUserRoleViewModel();
        public bool ShowMessage;
        public string? Message;


        [Parameter]
        public string? userId { get; set; }
        public bool _loading;


        protected async override Task OnInitializedAsync()
        {
            ShowMessage = false;
            UserID!.UserId = userId;
            UserRoleViewModel = await AdministrationService!.GetManageUserRoles(UserID!);
            RolesUsers = UserRoleViewModel.rolesUsers!;
            Mapper!.Map(UserRoleViewModel, EditUserRoleViewModel);
        }


        public bool _sortNameByLength;
        public SortMode _sortMode = SortMode.Multiple;

        //// custom sort by name length
        private Func<RolesUser, object> _sortBy => x =>
        {
            if (_sortNameByLength)
                return x.RoleId!;
            else
                return x.RoleName!;
        };


        public string? _searchString;
        public Func<RolesUser, bool> _quickFilter => x =>
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;

            if (x.RoleName!.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        };

        public void CheckEdite(RolesUser rolesUser)
        {
            RolesUsers.Add(rolesUser);
            EditUserRoleViewModel!.UserId = userId;
            UserRoleViewModel!.rolesUsers = RolesUsers;
            Mapper!.Map(EditUserRoleViewModel, UserRoleViewModel);
        }

        public async Task Edite()
        {
            await AdministrationService!.ManageUserRoles(UserRoleViewModel!);
            Submit();
        }





        [CascadingParameter] IMudDialogInstance? MudDialog { get; set; }

        public void Submit()
        {
            MudDialog!.Close(DialogResult.Ok(true));

        }

        public void Cancel()
        {
            MudDialog!.Cancel();
        }



    }
}

