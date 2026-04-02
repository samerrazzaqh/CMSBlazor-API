using System;
using CMSBlazor.Shared.ViewModels.Administration;

namespace CMSBlazor.Client.Service.Administration
{
	public interface IAdministrationService
	{
        Task<IEnumerable<Roles>?> ListRoles();
        Task<IEnumerable<Users>> ListUsers();

        Task<RoleViewModel?> GetEditRole(string roleId);
        Task<RoleViewModel?> EditRole(RoleViewModel roleViewModel);

        Task<UserRoleViewModel> GetEditUsersInRole(UserRoleViewModel userRoleViewModel);
        Task<UserRoleViewModel> EditUsersInRole(UserRoleViewModel userRoleViewModel);


        Task<CreateRoleViewModel> CreateRole(CreateRoleViewModel createRoleViewModel);
        Task<RoleViewModel> DeleteRole(RoleViewModel roleViewModel);
        Task<UserID> DeleteUser(UserID userID);


        Task<UserViewModel> GetEditUser(UserID userID);
        Task<UserViewModel> EditUser(UserViewModel userViewModel);


        Task<UserRoleViewModel> GetManageUserRoles(UserID userID);
        Task<UserRoleViewModel> ManageUserRoles(UserRoleViewModel userRoleViewModel);



        Task<UserClaimsViewModel> GetManageUserClaims(UserID userID);
        Task<UserClaimsViewModel> ManageUserClaims(UserClaimsViewModel userClaimsViewModel);




        Task<PolicyRole> GetClaimsByUser(PolicyRole policyRole);
        Task<PolicyRole> GetRolesByUser(PolicyRole policyRole);



    }
}

