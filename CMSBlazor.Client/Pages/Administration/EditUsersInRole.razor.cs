using System;
using System.Collections;
using AutoMapper;
using CMSBlazor.Client.Pages.Category;
using CMSBlazor.Client.Service.Administration;
using CMSBlazor.Client.Service.Categories;
using CMSBlazor.Shared.ViewModels.Administration;
using CMSBlazor.Shared.ViewModels.Post;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using MudBlazor;
using static MudBlazor.CategoryTypes;

namespace CMSBlazor.Client.Pages.Administration
{
	public partial class EditUsersInRole
	{

        [Inject]
        public IMapper? Mapper { get; set; }

        [Inject]
        public IAdministrationService? AdministrationService { get; set; }
        public UserRoleViewModel? UserRoleViewModel = new UserRoleViewModel();
        public EditUserRoleViewModel? EditUserRoleViewModel = new EditUserRoleViewModel();

        public List<UserRole> UserRoles = new List<UserRole>();


        public EditRoleViewModel? EditRoleViewModel = new EditRoleViewModel();
        public bool ShowMessage;
        public string? Message;


        [Parameter]
        public string? roleId { get; set; }
        public bool _loading;


        protected async override Task OnInitializedAsync()
        {
            ShowMessage = false;
            UserRoleViewModel!.RoleId = roleId;
            UserRoleViewModel = await AdministrationService!.GetEditUsersInRole(UserRoleViewModel!);
            UserRoles = UserRoleViewModel.userRoles!;
            Mapper!.Map(UserRoleViewModel, EditUserRoleViewModel);
        }


        public bool _sortNameByLength;
        public SortMode _sortMode = SortMode.Multiple;

        //// custom sort by name length
        private Func<UserRole, object> _sortBy => x =>
        {
            if (_sortNameByLength)
                return x.UserId!;
            else
                return x.UserName!;
        };


        public string? _searchString;
        public Func<UserRole, bool> _quickFilter => x =>
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;

            if (x.UserName!.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        };

        public void CheckEdite(UserRole userRole)
        {
            UserRoles.Add(userRole);
            EditUserRoleViewModel!.RoleId = roleId;
            UserRoleViewModel!.userRoles = UserRoles;
            Mapper!.Map(EditUserRoleViewModel, UserRoleViewModel);
        }

        public async Task Edite()
        {
            await AdministrationService!.EditUsersInRole(UserRoleViewModel!);
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

