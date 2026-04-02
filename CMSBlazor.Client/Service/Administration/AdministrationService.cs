using System;
using System.Net.Http.Headers;
using Blazored.LocalStorage;
using CMSBlazor.Client.Helpers;
using CMSBlazor.Client.Models;
using CMSBlazor.Shared.ViewModels.Administration;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CMSBlazor.Client.Service.Administration
{
	public class AdministrationService : IAdministrationService
    {

        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;




        public AdministrationService(HttpClient httpClient,
            AuthenticationStateProvider authenticationStateProvider,
            ILocalStorageService _localStorage)
		{
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            this._localStorage = _localStorage;
        }



        //==========================================================================================



        public async Task<IEnumerable<Roles>?> ListRoles()
        {
            LocalStorageModel localStorageModel = new LocalStorageModel();
            List<Roles> roles = null!;
            try
            {
                localStorageModel = await _localStorage.GetItemAsync<LocalStorageModel>("Storage");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                     "Bearer", localStorageModel.Token);

                var response = await _httpClient.GetAsync(CMSConstant.BaseApiAddress + "api/Administration/ListRoles");


                var content = await response.Content.ReadAsStringAsync();
                JObject jObject = JObject.Parse(content);
                JToken? jToken = jObject["roles"];

                var reslt = JsonConvert.DeserializeObject<List<Roles>>(jToken!.ToString());
                roles = reslt!;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                roles = null!;
            }
            return roles;
        }


        //==========================================================================================


        public async Task<IEnumerable<Users>> ListUsers()
        {
            LocalStorageModel localStorageModel = new LocalStorageModel();
            List<Users> users = null!;
            try
            {
                localStorageModel = await _localStorage.GetItemAsync<LocalStorageModel>("Storage");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                     "Bearer", localStorageModel.Token);

                var response = await _httpClient.GetAsync(CMSConstant.BaseApiAddress + "api/Administration/ListUsers");


                var content = await response.Content.ReadAsStringAsync();
                JObject jObject = JObject.Parse(content);
                JToken? jToken = jObject["users"];

                var reslt = JsonConvert.DeserializeObject<List<Users>>(jToken!.ToString());
                users = reslt!;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                users = null!;
            }
            return users;
        }


        //==========================================================================================


        public async Task<RoleViewModel?> GetEditRole(string roleId)
        {
            LocalStorageModel localStorageModel = new LocalStorageModel();
            RoleViewModel roleViewModel = null!;
            try
            {
                localStorageModel = await _localStorage.GetItemAsync<LocalStorageModel>("Storage");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                     "Bearer", localStorageModel.Token);

                var model = new RoleViewModel
                {
                    Id = roleId,
                };

                var json = JsonConvert.SerializeObject(model);
                HttpContent httpContent = new StringContent(json);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                var response = await _httpClient.PostAsync(CMSConstant.BaseApiAddress + "api/Administration/GetEditRole", httpContent);
                var content = await response.Content.ReadAsStringAsync();




                //Get Objects==============================================================
                JObject? jObject = JsonConvert.DeserializeObject<dynamic>(content);
                var success = jObject!.Value<bool>("success");
                var message = jObject.Value<string>("message");
                var rolename = jObject.Value<string>("name");
                var roleid = jObject.Value<string>("id");
                JToken jToken = jObject["users"]!;
                var users = JsonConvert.DeserializeObject<List<string>>(jToken.ToString());
                //var user = jObject.Value<List<string>>("users");


                var _roleViewModel = new RoleViewModel
                {
                    Successful = success,
                    Message = message,

                    Id = roleid,
                    RoleName = rolename,
                    Users = users,
                };
                roleViewModel = _roleViewModel;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                roleViewModel = null!;
            }
            return roleViewModel;
        }




        //==========================================================================================

        public async Task<RoleViewModel?> EditRole(RoleViewModel roleViewModel)
        {
            LocalStorageModel localStorageModel = new LocalStorageModel();
            try
            {
                localStorageModel = await _localStorage.GetItemAsync<LocalStorageModel>("Storage");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                     "Bearer", localStorageModel.Token);

                //var model = new RoleViewModel
                //{
                //    Id = roleId,
                //};

                var json = JsonConvert.SerializeObject(roleViewModel);
                HttpContent httpContent = new StringContent(json);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                var response = await _httpClient.PostAsync(CMSConstant.BaseApiAddress + "api/Administration/EditRole", httpContent);
                var content = await response.Content.ReadAsStringAsync();




                //Get Objects==============================================================
                JObject? jObject = JsonConvert.DeserializeObject<dynamic>(content);
                var success = jObject!.Value<bool>("success");
                var message = jObject.Value<string>("message");
                var rolename = jObject.Value<string>("name");
                var roleid = jObject.Value<string>("id");
                JToken jToken = jObject["users"]!;
                var users = JsonConvert.DeserializeObject<List<string>>(jToken.ToString());

                roleViewModel = null!;
                var _roleViewModel = new RoleViewModel
                {
                    Successful = success,
                    Message = message,

                    Id = roleid,
                    RoleName = rolename,
                    Users = users,
                };
                roleViewModel = _roleViewModel;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                roleViewModel = null!;
            }
            return roleViewModel;
        }




        //==========================================================================================


        public async Task<UserRoleViewModel> GetEditUsersInRole(UserRoleViewModel userRoleViewModel)
        {
            LocalStorageModel localStorageModel = new LocalStorageModel();
            try
            {
                localStorageModel = await _localStorage.GetItemAsync<LocalStorageModel>("Storage");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                     "Bearer", localStorageModel.Token);

                
                var json = JsonConvert.SerializeObject(userRoleViewModel);
                HttpContent httpContent = new StringContent(json);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                var response = await _httpClient.PostAsync(CMSConstant.BaseApiAddress + "api/Administration/GetEditUsersInRole", httpContent);
                var content = await response.Content.ReadAsStringAsync();




                //Get Objects==============================================================
                JObject? jObject = JsonConvert.DeserializeObject<dynamic>(content);
                var success = jObject!.Value<bool>("success");
                var message = jObject.Value<string>("message");
                JToken jToken = jObject["listUserRole"]!;
                var listUserRole = JsonConvert.DeserializeObject<List<UserRole>>(jToken.ToString());
               
                userRoleViewModel = null!;

                var _userRoleViewModel = new UserRoleViewModel
                {
                    Successful = success,
                    Message = message,

                    userRoles = listUserRole,
                };
                userRoleViewModel = _userRoleViewModel;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                userRoleViewModel = null!;
            }
            return userRoleViewModel;
        }



        //==========================================================================================


        public async Task<UserRoleViewModel> EditUsersInRole(UserRoleViewModel userRoleViewModel)
        {
            LocalStorageModel localStorageModel = new LocalStorageModel();
            try
            {
                localStorageModel = await _localStorage.GetItemAsync<LocalStorageModel>("Storage");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                     "Bearer", localStorageModel.Token);


                var json = JsonConvert.SerializeObject(userRoleViewModel);
                HttpContent httpContent = new StringContent(json);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                var response = await _httpClient.PostAsync(CMSConstant.BaseApiAddress + "api/Administration/EditUsersInRole", httpContent);
                var content = await response.Content.ReadAsStringAsync();




                //Get Objects==============================================================
                JObject? jObject = JsonConvert.DeserializeObject<dynamic>(content);
                var success = jObject!.Value<bool>("success");
                var message = jObject.Value<string>("message");
                var id = jObject.Value<string>("id");

                userRoleViewModel = null!;

                var _userRoleViewModel = new UserRoleViewModel
                {
                    Successful = success,
                    Message = message,

                    RoleId = id,
                };
                userRoleViewModel = _userRoleViewModel;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                userRoleViewModel = null!;
            }
            return userRoleViewModel;
        }




        //==========================================================================================


        public async Task<CreateRoleViewModel> CreateRole(CreateRoleViewModel createRoleViewModel)
        {
            LocalStorageModel localStorageModel = new LocalStorageModel();
            localStorageModel = await _localStorage.GetItemAsync<LocalStorageModel>("Storage");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                 "Bearer", localStorageModel.Token);

            var json = JsonConvert.SerializeObject(createRoleViewModel);
            HttpContent httpContent = new StringContent(json);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");


            var result = await _httpClient.PostAsync(CMSConstant.BaseApiAddress + "api/Administration/CreateRole", httpContent);

            var content = await result.Content.ReadAsStringAsync();
            JObject jObject = JsonConvert.DeserializeObject<dynamic>(content)!;

            var success = jObject.Value<bool>("success");

            if (success == true)
            {
                createRoleViewModel.Message = jObject.Value<string>("message");
                createRoleViewModel.Successful = success;
            }
            else
            {
                createRoleViewModel.Message = jObject.Value<string>("message");
                createRoleViewModel.Successful = success;
            }
            return createRoleViewModel;
        }

        //==========================================================================================

        public async Task<RoleViewModel> DeleteRole(RoleViewModel roleViewModel)
        {
            LocalStorageModel localStorageModel = new LocalStorageModel();
            localStorageModel = await _localStorage.GetItemAsync<LocalStorageModel>("Storage");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                 "Bearer", localStorageModel.Token);

            var json = JsonConvert.SerializeObject(roleViewModel);
            HttpContent httpContent = new StringContent(json);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");


            var result = await _httpClient.PostAsync(CMSConstant.BaseApiAddress + "api/Administration/DeleteRole", httpContent);

            var content = await result.Content.ReadAsStringAsync();
            JObject jObject = JsonConvert.DeserializeObject<dynamic>(content)!;

            var success = jObject.Value<bool>("success");

            if (success == true)
            {
                roleViewModel.Message = jObject.Value<string>("message");
                roleViewModel.Successful = success;
            }
            else
            {
                roleViewModel.Message = jObject.Value<string>("message");
                roleViewModel.Successful = success;
            }
            return roleViewModel;
        }



        //==========================================================================================


        public async Task<UserID> DeleteUser(UserID userID)
        {
            LocalStorageModel localStorageModel = new LocalStorageModel();
            localStorageModel = await _localStorage.GetItemAsync<LocalStorageModel>("Storage");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                 "Bearer", localStorageModel.Token);

            var json = JsonConvert.SerializeObject(userID);
            HttpContent httpContent = new StringContent(json);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");


            var result = await _httpClient.PostAsync(CMSConstant.BaseApiAddress + "api/Administration/DeleteUser", httpContent);

            var content = await result.Content.ReadAsStringAsync();
            JObject jObject = JsonConvert.DeserializeObject<dynamic>(content)!;

            var success = jObject.Value<bool>("success");

            if (success == true)
            {
                userID.Message = jObject.Value<string>("message");
                userID.Successful = success;
            }
            else
            {
                userID.Message = jObject.Value<string>("message");
                userID.Successful = success;
            }
            return userID;
        }




        //==========================================================================================

        public async Task<UserViewModel> GetEditUser(UserID userID)
        {
            LocalStorageModel localStorageModel = new LocalStorageModel();
            UserViewModel userViewModel = null!;
            try
            {
                localStorageModel = await _localStorage.GetItemAsync<LocalStorageModel>("Storage");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                     "Bearer", localStorageModel.Token);


                var json = JsonConvert.SerializeObject(userID);
                HttpContent httpContent = new StringContent(json);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                var response = await _httpClient.PostAsync(CMSConstant.BaseApiAddress + "api/Administration/GetEditUser", httpContent);
                var content = await response.Content.ReadAsStringAsync();




                //Get Objects==============================================================
                JObject? jObject = JsonConvert.DeserializeObject<dynamic>(content);
                var success = jObject!.Value<bool>("success");
                var message = jObject.Value<string>("message");
                var model = jObject.Value<object>("model");
                JObject? jObject_model = JsonConvert.DeserializeObject<dynamic>(model!.ToString()!);
                var id = jObject_model!.Value<string>("id");
                var userName = jObject_model!.Value<string>("userName");
                var email = jObject_model!.Value<string>("email");
                var block = jObject_model!.Value<bool>("block");

                JToken claims = jObject_model["claims"]!;
                var listclaims = JsonConvert.DeserializeObject<List<string>>(claims.ToString());

                JToken roles = jObject_model["roles"]!;
                var listroles = JsonConvert.DeserializeObject<List<string>>(roles.ToString());



                var _userViewModel = new UserViewModel
                {
                    Successful = success,
                    Message = message,

                    Id = id,
                    UserName = userName!,
                    Email = email,
                    Block= block,
                    Claims = listclaims!,
                    Roles = listroles!
                };
                userViewModel = _userViewModel;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                userViewModel = null!;
            }
            return userViewModel;
        }


        //==========================================================================================
        public async Task<UserViewModel> EditUser(UserViewModel userViewModel)
        {
            LocalStorageModel localStorageModel = new LocalStorageModel();
           
            try
            {
                localStorageModel = await _localStorage.GetItemAsync<LocalStorageModel>("Storage");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                     "Bearer", localStorageModel.Token);


                var json = JsonConvert.SerializeObject(userViewModel);
                HttpContent httpContent = new StringContent(json);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                var response = await _httpClient.PostAsync(CMSConstant.BaseApiAddress + "api/Administration/EditUser", httpContent);
                var content = await response.Content.ReadAsStringAsync();




                //Get Objects==============================================================
                JObject? jObject = JsonConvert.DeserializeObject<dynamic>(content);
                var success = jObject!.Value<bool>("success");
                var message = jObject.Value<string>("message");

                var _userViewModel = new UserViewModel
                {
                    Successful = success,
                    Message = message,
                };
                userViewModel = _userViewModel;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                userViewModel = null!;
            }
            return userViewModel;
        }


        //==========================================================================================

        public async Task<UserRoleViewModel> GetManageUserRoles(UserID userID)
        {
            LocalStorageModel localStorageModel = new LocalStorageModel();
            UserRoleViewModel userRoleViewModel = null!;
            try
            {
                localStorageModel = await _localStorage.GetItemAsync<LocalStorageModel>("Storage");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                     "Bearer", localStorageModel.Token);


                var json = JsonConvert.SerializeObject(userID);
                HttpContent httpContent = new StringContent(json);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                var response = await _httpClient.PostAsync(CMSConstant.BaseApiAddress + "api/Administration/GetManageUserRoles", httpContent);
                var content = await response.Content.ReadAsStringAsync();




                //Get Objects==============================================================
                JObject? jObject = JsonConvert.DeserializeObject<dynamic>(content);
                var success = jObject!.Value<bool>("success");
                var message = jObject.Value<string>("message");
                JToken model = jObject["model"]!;
                var rolesUser = JsonConvert.DeserializeObject<List<RolesUser>>(model!.ToString());

                var _userRoleViewModel = new UserRoleViewModel
                {
                    Successful = success,
                    Message = message,

                    rolesUsers = rolesUser
                };
                userRoleViewModel = _userRoleViewModel;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                userRoleViewModel = null!;
            }
            return userRoleViewModel;
        }
        //==========================================================================================
        public async Task<UserRoleViewModel> ManageUserRoles(UserRoleViewModel userRoleViewModel)
        {
            LocalStorageModel localStorageModel = new LocalStorageModel();
            try
            {
                localStorageModel = await _localStorage.GetItemAsync<LocalStorageModel>("Storage");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                     "Bearer", localStorageModel.Token);


                var json = JsonConvert.SerializeObject(userRoleViewModel);
                HttpContent httpContent = new StringContent(json);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                var response = await _httpClient.PostAsync(CMSConstant.BaseApiAddress + "api/Administration/ManageUserRoles", httpContent);
                var content = await response.Content.ReadAsStringAsync();




                //Get Objects==============================================================
                JObject? jObject = JsonConvert.DeserializeObject<dynamic>(content);
                var success = jObject!.Value<bool>("success");
                var message = jObject.Value<string>("message");
                var _userRoleViewModel = new UserRoleViewModel
                {
                    Successful = success,
                    Message = message,
                };
                userRoleViewModel = _userRoleViewModel;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                userRoleViewModel = null!;
            }
            return userRoleViewModel;
        }


        //==========================================================================================



        public async Task<UserClaimsViewModel> GetManageUserClaims(UserID userID)
        {
            LocalStorageModel localStorageModel = new LocalStorageModel();
            UserClaimsViewModel userClaimsViewModel = null!;
            try
            {
                localStorageModel = await _localStorage.GetItemAsync<LocalStorageModel>("Storage");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                     "Bearer", localStorageModel.Token);


                var json = JsonConvert.SerializeObject(userID);
                HttpContent httpContent = new StringContent(json);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                var response = await _httpClient.PostAsync(CMSConstant.BaseApiAddress + "api/Administration/GetManageUserClaims", httpContent);
                var content = await response.Content.ReadAsStringAsync();




                //Get Objects==============================================================
                JObject? jObject = JsonConvert.DeserializeObject<dynamic>(content);
                var success = jObject!.Value<bool>("success");
                var message = jObject.Value<string>("message");
                var model = jObject.Value<object>("model");
                JObject? jObject_model = JsonConvert.DeserializeObject<dynamic>(model!.ToString()!);
                var userId = jObject_model!.Value<string>("userId");
                JToken cliams = jObject_model["cliams"]!;
                var listcliams = JsonConvert.DeserializeObject<List<UserClaim>>(cliams!.ToString());

                var _userClaimsViewModel = new UserClaimsViewModel
                {
                    Successful = success,
                    Message = message,

                    UserId = userId,
                    Cliams = listcliams!
                };
                userClaimsViewModel = _userClaimsViewModel;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                userClaimsViewModel = null!;
            }
            return userClaimsViewModel;
        }


        //==========================================================================================

        public async Task<UserClaimsViewModel> ManageUserClaims(UserClaimsViewModel userClaimsViewModel)
        {
            LocalStorageModel localStorageModel = new LocalStorageModel();
            try
            {
                localStorageModel = await _localStorage.GetItemAsync<LocalStorageModel>("Storage");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                     "Bearer", localStorageModel.Token);


                var json = JsonConvert.SerializeObject(userClaimsViewModel);
                HttpContent httpContent = new StringContent(json);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                var response = await _httpClient.PostAsync(CMSConstant.BaseApiAddress + "api/Administration/ManageUserClaims", httpContent);
                var content = await response.Content.ReadAsStringAsync();




                //Get Objects==============================================================
                JObject? jObject = JsonConvert.DeserializeObject<dynamic>(content);
                var success = jObject!.Value<bool>("success");
                var message = jObject.Value<string>("message");

                var _userClaimsViewModel = new UserClaimsViewModel
                {
                    Successful = success,
                    Message = message,
                };
                userClaimsViewModel = _userClaimsViewModel;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                userClaimsViewModel = null!;
            }
            return userClaimsViewModel;
        }






        public async Task<PolicyRole> GetClaimsByUser(PolicyRole policyRole)
        {
            LocalStorageModel localStorageModel = new LocalStorageModel();
            
            try
            {
                localStorageModel = await _localStorage.GetItemAsync<LocalStorageModel>("Storage");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                     "Bearer", localStorageModel.Token);


                var json = JsonConvert.SerializeObject(policyRole);
                HttpContent httpContent = new StringContent(json);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                var response = await _httpClient.PostAsync(CMSConstant.BaseApiAddress + "api/Administration/GetClaimsByUser", httpContent);
                var content = await response.Content.ReadAsStringAsync();




                //Get Objects==============================================================
                JObject? jObject = JsonConvert.DeserializeObject<dynamic>(content);
                var success = jObject!.Value<bool>("success");
                var message = jObject.Value<string>("message");
                JToken claims = jObject["claims"]!;
                var listcliams = JsonConvert.DeserializeObject<List<Policy>>(claims!.ToString());

                policyRole = null!;

                var _policyRole = new PolicyRole
                {
                    Successful = success,
                    Message = message,

                    policies = listcliams

                };
                policyRole = _policyRole;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                policyRole = null!;
            }
            return policyRole;
        }

        public async Task<PolicyRole> GetRolesByUser(PolicyRole policyRole)
        {
            LocalStorageModel localStorageModel = new LocalStorageModel();

            try
            {
                localStorageModel = await _localStorage.GetItemAsync<LocalStorageModel>("Storage");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                     "Bearer", localStorageModel.Token);


                var json = JsonConvert.SerializeObject(policyRole);
                HttpContent httpContent = new StringContent(json);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                var response = await _httpClient.PostAsync(CMSConstant.BaseApiAddress + "api/Administration/GetRolesByUser", httpContent);
                var content = await response.Content.ReadAsStringAsync();




                //Get Objects==============================================================
                JObject? jObject = JsonConvert.DeserializeObject<dynamic>(content);
                var success = jObject!.Value<bool>("success");
                var message = jObject.Value<string>("message");
                JToken roles = jObject["roles"]!;
                var listrole = JsonConvert.DeserializeObject<List<Role>>(roles!.ToString());

                policyRole = null!;

                var _policyRole = new PolicyRole
                {
                    Successful = success,
                    Message = message,

                    roles = listrole

                };
                policyRole = _policyRole;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                policyRole = null!;
            }
            return policyRole;
        }






    }
}

