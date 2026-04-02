using System;
using System.Data;
using System.Security.Claims;
using CMSBlazor.Data;
using CMSBlazor.Shared.Models;
using CMSBlazor.Shared.ViewModels.Account;
using CMSBlazor.Shared.ViewModels.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMSBlazor.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class AdministrationController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly AppDbContext _context;

        public AdministrationController(AppDbContext context,
                                        SignInManager<ApplicationUser> signInManager,
                                        RoleManager<IdentityRole> roleManager,
                                        UserManager<ApplicationUser> userManager)
		{
            _context = context;
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }




        [HttpPost("GetClaimsByUser")]
        public async Task<ActionResult> GetClaimsByUser(PolicyRole policyRole)
        {
            var userId = await userManager.FindByIdAsync(policyRole.UserId!);


            if (userId == null)
            {
                return Ok(new { success = false, message = $"User cannot be found" });
            }


            var Claims = (from p in _context.UserClaims.Where(x => x.UserId == userId.Id)
                          select new
                        {
                            ClaimType = _context.UserClaims.Where(x => x.Id == p.Id).Select(e => e.ClaimType).FirstOrDefault(),
                            ClaimValue = _context.UserClaims.Where(x => x.Id == p.Id).Select(e => e.ClaimValue).FirstOrDefault(),

                        }).ToListAsync();


            return Ok(new { success = true, message = "GetClaimsByUser", claims = await Claims });

        }

        [HttpPost("GetRolesByUser")]
        public async Task<ActionResult> GetRolesByUser(PolicyRole policyRole)
        {
            var userId = await userManager.FindByIdAsync(policyRole.UserId!);


            if (userId == null)
            {
                return Ok(new { success = false, message = $"User cannot be found" });
            }



            var Role = (from p in _context.UserRoles.Where(x => x.UserId == userId.Id)
                        select new
                        {
                            RoleName = _context.Roles.Where(x => x.Id == p.RoleId).Select(e => e.Name).FirstOrDefault(),

                        }).ToListAsync();


            return Ok(new { success = true, message = "GetRolesByUser", roles = await Role });

        }















        [HttpGet("ListRoles")]
        public async Task<ActionResult> ListRoles()
        {
            var roles = await roleManager.Roles.ToListAsync();
            return Ok(new { success = true, roles, message = "ListRoles" });
            
        }


        
        [HttpGet("ListUsers")]
        public async Task<ActionResult> ListUsers()
        {
            var users = await userManager.Users.ToListAsync();
            return Ok(new { success = true, users, message = "ListUsers" });

        }




        
        [HttpPost("GetEditRole")]
        public async Task<ActionResult<RoleViewModel>> GetEditRole(RoleViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id!);
            

            if (role == null)
            {
                return Ok(new { success = false, message = $"Role with Id = {model.Id} cannot be found" });
            }
            //role.Name = model.RoleName;
            // Retrieve all the Users
            //foreach (var user in userManager.Users.ToList())
            //{
            //    if (await userManager.IsInRoleAsync(user, role.Name!))
            //    {
            //        model.Users!.Add(user.UserName!);
            //    }
            //}

            var UserId = _context.UserRoles.Where(e => e.RoleId == model.Id).Select(e => e.UserId);
            foreach (var id in UserId.ToList())
            {
                var username = _context.Users.Where(e => e.Id == id).Select(e => e.UserName).FirstOrDefault();
                model.Users!.Add(username!);
            }



            model.RoleName = role.Name;

            return Ok(new { success = true, message = "GetRoleUsers", model.Users, role.Name,role.Id });

        }

   

        [HttpPost("EditRole")]
        public async Task<ActionResult<RoleViewModel>> EditRole(RoleViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id!);

            if (role == null)
            {
                return Ok(new { success = false, message = $"Role with Id = {model.Id} cannot be found" });
            }
            else
            {
                role.Name = model.RoleName;

                // Update the Role using UpdateAsync
                var result = await roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    // Retrieve all the Users
                    foreach (var user in userManager.Users.ToList())
                    {
                        if (await userManager.IsInRoleAsync(user, role.Name!))
                        {
                            model.Users!.Add(user.UserName!);
                        }
                    }

                    return Ok(new { success = true, message = "Update", model.Users, role.Name, role.Id });
                }
                role.Name = role.Name;
                return Ok(new { success = false, message = "Error Update is already taken", model.Users, role.Name, role.Id });
            }

        }




        [HttpPost("GetEditUsersInRole")]
        public async Task<ActionResult<UserRoleViewModel>> GetEditUsersInRole(UserRoleViewModel userRoleViewModel)
        {

            var role = await roleManager.FindByIdAsync(userRoleViewModel.RoleId!);

            if (role == null)
            {
                return Ok(new { success = false, message = $"Role with Id = {userRoleViewModel.RoleId} cannot be found" });
            }

            var listUserRole = new List<UserRole>();

            foreach (var user in userManager.Users.ToList())
            {
                var userRole = new UserRole
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await userManager.IsInRoleAsync(user, role.Name!))
                {
                    userRole.IsSelected = true;
                }
                else
                {
                    userRole.IsSelected = false;
                }

                listUserRole.Add(userRole);
            }
            return Ok(new { success = true, listUserRole, message = "GetUsersInRole" });
        }


        [HttpPost("EditUsersInRole")]
        public async Task<ActionResult<UserRoleViewModel>> EditUsersInRole(UserRoleViewModel userRoleViewModel )
        {
            //List<UserRole> model,
            var role = await roleManager.FindByIdAsync(userRoleViewModel.RoleId!);

            if (role == null)
            {
                return Ok(new { success = false, message = $"Role with Id = {userRoleViewModel.RoleId} cannot be found" });
            }

            for (int i = 0; i < userRoleViewModel.userRoles!.Count; i++)
            {
                var user = await userManager.FindByIdAsync(userRoleViewModel.userRoles[i].UserId!);

                IdentityResult result = null!;

                if (userRoleViewModel.userRoles[i].IsSelected && !(await userManager.IsInRoleAsync(user!, role.Name!)))
                {
                    result = await userManager.AddToRoleAsync(user!, role.Name!);
                }
                else if (!userRoleViewModel.userRoles[i].IsSelected && await userManager.IsInRoleAsync(user!, role.Name!))
                {
                    result = await userManager.RemoveFromRoleAsync(user!, role.Name!);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (userRoleViewModel.userRoles.Count - 1))
                        continue;
                    else
                        return Ok(new { success = true, role.Id, message = "EditRole Ok" });
                }
            }

            return Ok(new { success = true, role.Id, message = "EditRole Ok" });
        }



        [HttpPost("CreateRole")]
        public async Task<ActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                // We just need to specify a unique role name to create a new role
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName

                };

                // Saves the role in the underlying AspNetRoles table
                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return Ok(new { success = true, message = "CreateRole" });
                }

                return Ok(new { success = false, message = $"Error CreateRole {identityRole.Name} is already taken" });
            }

            return Ok(new { success = false, message = "Error Model" });
        }



        [HttpPost("DeleteRole")]
        public async Task<ActionResult> DeleteRole(RoleViewModel roleViewModel)
        {
            var role = await roleManager.FindByIdAsync(roleViewModel.Id!);

            if (role == null)
            {
                return Ok(new { success = false, message = $"Role with Id cannot be found" });
            }
            else
            {
                try
                {

                    var result = await roleManager.DeleteAsync(role);

                    if (result.Succeeded)
                    {
                        return Ok(new { success = true, message = "DeleteRole" });
                    }

                    return Ok(new { success = false, message = "Error DeleteRole" });
                }
                catch (DbUpdateException)
                {
                    var ErrorTitle = $"{role.Name} role is in use";
                    var ErrorMessage = $"{role.Name} role cannot be deleted as there are users in this role. If you want to delete this role, please remove the users from the role and then try to delete";
                    return Ok(new { success = false, message = $"Error DeleteRole {ErrorTitle}  {ErrorMessage}" });
                }
            }
        }


        [HttpPost("DeleteUser")]
        public async Task<ActionResult> DeleteUser(UserID userID)
        {
            var user = await userManager.FindByIdAsync(userID.UserId!);

            if (user == null)
            {
                return Ok(new { success = false, message = $"User with Id = {userID.UserId} cannot be found" });
            }
            else
            {
                try
                {

                    var result = await userManager.DeleteAsync(user);

                    if (result.Succeeded)
                    {
                        return Ok(new { success = true, message = $"DeleteUser" });
                    }

                    return Ok(new { success = false, message = "Error DeleteUser" });
                }
                catch (DbUpdateException)
                {
                    var ErrorTitle = $"{user.UserName} Has Permission";
                    var ErrorMessage = $"{user.UserName}  cannot be deleted because it has permission. If you want to delete this user, please remove the permission from the user and then try to delete";
                    return Ok(new { success = false, message = $"Error DeleteUser \n {ErrorTitle} \n {ErrorMessage}" });
                }
            }
        }





        [HttpPost("GetEditUser")]
        public async Task<ActionResult> GetEditUser(UserID userID)
        {

            var user = await userManager.FindByIdAsync(userID.UserId!);

            if (user == null)
            {
                return Ok(new { success = false, message = $"User with Id = {userID.UserId} cannot be found" });
            }

            // GetClaimsAsync retunrs the list of user Claims
            var userClaims = await userManager.GetClaimsAsync(user);
            // GetRolesAsync returns the list of user Roles
            var userRoles = await userManager.GetRolesAsync(user);


            if (DateTime.Now >= user.BlockTime)
            {
                user.Block = false;
                user.BlockTime = null;
                await userManager.UpdateAsync(user);
            }

            var model = new UserViewModel
            {
                Id = user.Id,
                Email = user.Email!,
                UserName = user.UserName!,
                Block = user.Block,
                Claims = userClaims.Select(c => c.Type + " : " + c.Value).ToList(),
                Roles = userRoles.ToList()
            };

            return Ok(new { success = true, model, message = $"GetEditUser" });
        }





        [HttpPost("EditUser")]
        public async Task<ActionResult> EditUser(UserViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id!);


            if (user == null)
            {
                return Ok(new { success = false, message = $"User with Id = {model.Id} cannot be found" });
            }

            user.Block = model.Block;

            if (model.Block == false)
            {
                user.Block = false;
                user.BlockTime = null;
            }
            else
            {
                user.Block = true;
                user.BlockTime = DateTime.Now.AddMinutes(1);
            }

            var result = await userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok(new { success = true, message = $" Edit User = {model.Email}" });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return Ok(new { success = false, model, message = $"Error EditUser" });

        }




        [HttpPost("GetManageUserRoles")]
        //[Authorize(Policy = "EditRolePolicy")]
        public async Task<ActionResult> GetManageUserRoles(UserID userID)
        {
            var user = await userManager.FindByIdAsync(userID.UserId!);

            if (user == null)
            {
                return Ok(new { success = false, message = $"User with Id = {userID.UserId} cannot be found" });
            }

            var model = new List<RolesUser>();

            foreach (var role in roleManager.Roles.ToList())
            {
                var userRolesViewModel = new RolesUser
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };

                if (await userManager.IsInRoleAsync(user, role.Name!))
                {
                    userRolesViewModel.IsSelected = true;
                }
                else
                {
                    userRolesViewModel.IsSelected = false;
                }

                model.Add(userRolesViewModel);
            }
            return Ok(new { success = true, model, message = $"GetManageUserRoles" });
        }


        [HttpPost("ManageUserRoles")]
        //[Authorize(Policy = "EditRolePolicy")]
        public async Task<ActionResult> ManageUserRoles(UserRoleViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.UserId!);

            if (user == null)
            {
                return Ok(new { success = false, message = $"User with Id = {model.UserId} cannot be found" });
            }

            var roles = await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                return Ok(new { success = false, message = $"Cannot remove user existing roles" });
            }

            result = await userManager.AddToRolesAsync(user,
                model.rolesUsers!.Where(x => x.IsSelected).Select(y => y.RoleName)!);

            if (!result.Succeeded)
            {
                return Ok(new { success = false, message = $"Cannot add selected roles to user" });
            }
            return Ok(new { success = true, message = $"ManageUserRoles EditUser" });
        }



        [HttpPost("GetManageUserClaims")]
        public async Task<ActionResult> GetManageUserClaims(UserID userID)
        {
            var user = await userManager.FindByIdAsync(userID.UserId!);

            if (user == null)
            {
                return Ok(new { success = false, message = $"User with Id = {userID.UserId} cannot be found" });
            }

            // UserManager service GetClaimsAsync method gets all the current claims of the user
            var existingUserClaims = await userManager.GetClaimsAsync(user);

            var model = new UserClaimsViewModel
            {
                UserId = userID.UserId!
            };

            // Loop through each claim we have in our application
            foreach (Claim claim in ClaimsStore.AllClaims)
            {
                UserClaim userClaim = new UserClaim
                {
                    ClaimType = claim.Type
                };

                // If the user has the claim, set IsSelected property to true, so the checkbox
                // next to the claim is checked on the UI
                if (existingUserClaims.Any(c => c.Type == claim.Type && c.Value == "true"))
                {
                    userClaim.IsSelected = true;
                }

                model.Cliams.Add(userClaim);
            }
            return Ok(new { success = true, model, message = $"GetManageUserClaims" });

        }



        [HttpPost("ManageUserClaims")]
        public async Task<ActionResult> ManageUserClaims(UserClaimsViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.UserId!);

            if (user == null)
            {
                return Ok(new { success = false, message = $"User with Id = {model.UserId} cannot be found" });
            }

            // Get all the user existing claims and delete them
            var claims = await userManager.GetClaimsAsync(user);
            var result = await userManager.RemoveClaimsAsync(user, claims);

            if (!result.Succeeded)
            {
                return Ok(new { success = false, message = $"Cannot remove user existing claims" });
            }

            // Add all the claims that are selected on the UI
            result = await userManager.AddClaimsAsync(user,
                model.Cliams.Select(c => new Claim(c.ClaimType!, c.IsSelected ? "true" : "false")));

            if (!result.Succeeded)
            {
                return Ok(new { success = false, message = $"Cannot add selected claims to user" });
            }
            return Ok(new { success = true, message = $"ManageUserClaims" });

        }







    }
}

