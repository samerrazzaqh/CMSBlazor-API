using System;
using AutoMapper;
using CMSBlazor.Client.Service.Administration;
using CMSBlazor.Shared.ViewModels.Administration;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CMSBlazor.Client.Pages.Administration
{
	public partial class ManageUserClaims
	{
        [Inject]
        public IMapper? Mapper { get; set; }

        [Inject]
        public IAdministrationService? AdministrationService { get; set; }
        public UserID? UserID = new UserID();
        public UserClaimsViewModel? UserClaimsViewModel = new UserClaimsViewModel();

        public List<UserClaim> UserClaims = new List<UserClaim>();


        public EditUserClaimsViewModel? EditUserClaimsViewModel = new EditUserClaimsViewModel();
        public bool ShowMessage;
        public string? Message;


        [Parameter]
        public string? userId { get; set; }
        public bool _loading;


        protected async override Task OnInitializedAsync()
        {
            ShowMessage = false;
            UserID!.UserId = userId;
            UserClaimsViewModel = await AdministrationService!.GetManageUserClaims(UserID!);
            UserClaims = UserClaimsViewModel.Cliams!;
            Mapper!.Map(UserClaimsViewModel, EditUserClaimsViewModel);
        }


        public bool _sortNameByLength;
        public SortMode _sortMode = SortMode.Multiple;

        //// custom sort by name length
        private Func<UserClaim, object> _sortBy => x =>
        {
            if (_sortNameByLength)
                return x.ClaimType!;
            else
                return x.IsSelected!;
        };


        public string? _searchString;
        public Func<UserClaim, bool> _quickFilter => x =>
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;

            if (x.ClaimType!.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        };

        public void CheckEdite(UserClaim userClaim)
        {
            UserClaims.Add(userClaim);
            EditUserClaimsViewModel!.UserId = userId;
            UserClaimsViewModel!.Cliams = UserClaims;
            Mapper!.Map(EditUserClaimsViewModel, UserClaimsViewModel);
        }

        public async Task Edite()
        {
            await AdministrationService!.ManageUserClaims(UserClaimsViewModel!);
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

