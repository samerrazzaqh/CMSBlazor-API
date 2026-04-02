using System;
using System.Diagnostics;
using System.Net.Http.Headers;
using Blazored.LocalStorage;
using CMSBlazor.Client.Helpers;
using CMSBlazor.Client.Models;
using CMSBlazor.Shared.ViewModels.Profile;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CMSBlazor.Client.Service.Profile
{
	public class ProfileService : IProfileService
    {

        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;
        public ProfileService(HttpClient httpClient,
            AuthenticationStateProvider authenticationStateProvider,
            ILocalStorageService _localStorage)
		{
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            this._localStorage = _localStorage;
        }

      

        //==================================GetUserInfo=============================================
        public async Task<InfoProfile?> GetInfoProfile(string userId)
        {
            LocalStorageModel localStorageModel = new LocalStorageModel();
            InfoProfile infoProfile = null!;
            try
            {
                localStorageModel = await _localStorage.GetItemAsync<LocalStorageModel>("Storage");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                     "Bearer", localStorageModel.Token);


                var model = new InfoProfile
                {
                    UserId = userId,
                };

                var json = JsonConvert.SerializeObject(model);
                HttpContent httpContent = new StringContent(json);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await _httpClient.PostAsync(CMSConstant.BaseApiAddress + "api/Profile/Index", httpContent);

                var content = await response.Content.ReadAsStringAsync();


                //Get Objects==============================================================
                JObject? jObject = JsonConvert.DeserializeObject<dynamic>(content);
                var aboutUser = jObject!.Value<object>("aboutUser");
                JObject? jObject_aboutUser = JsonConvert.DeserializeObject<dynamic>(aboutUser!.ToString()!);
                var applicationUsers = jObject_aboutUser!.Value<object>("applicationUsers");
                JObject? jObject_applicationUsers = JsonConvert.DeserializeObject<dynamic>(applicationUsers!.ToString()!);


                var success = jObject.Value<bool>("success");
                var profession = jObject_aboutUser.Value<string>("profession");


                var _infoUser = new InfoProfile
                {
                    UserId = jObject_applicationUsers!.Value<string>("id"),
                    FirstName = jObject_applicationUsers.Value<string>("firstName"),
                    LastName = jObject_applicationUsers.Value<string>("lastName"),
                    Email = jObject_applicationUsers.Value<string>("email"),
                    PhoneNumber = jObject_applicationUsers.Value<string>("phoneNumber"),
                    UserName = jObject_applicationUsers.Value<string>("userName"),
                    Password = jObject_applicationUsers.Value<string>("passwordHash"),



                    UrlImageProfile = jObject_aboutUser.Value<string>("urlImageProfile"),
                    UrlImageCover = jObject_aboutUser.Value<string>("urlImageCover"),
                    Profession = jObject_aboutUser.Value<string>("profession"),
                    DateOfBurthEdite = jObject_aboutUser.Value<DateTime>("dateOfBurth"),
                    Location = jObject_aboutUser.Value<string>("location"),
                    Skills = jObject_aboutUser.Value<string>("skills"),
                    WorkLink = jObject_aboutUser.Value<string>("workLink"),
                    Experience = jObject_aboutUser.Value<string>("experience")
                };
                infoProfile = _infoUser;



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                infoProfile = null!;
            }
            return infoProfile;
        }



        public async Task<InfoProfile?> EditInfoProfile(InfoProfile infoProfile)
        {

            LocalStorageModel localStorageModel = new LocalStorageModel();
            
            try
            {
                localStorageModel = await _localStorage.GetItemAsync<LocalStorageModel>("Storage");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                     "Bearer", localStorageModel.Token);


                var json = JsonConvert.SerializeObject(infoProfile);
                HttpContent httpContent = new StringContent(json);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await _httpClient.PostAsync(CMSConstant.BaseApiAddress + "api/Profile/EditProfile", httpContent);

                var content = await response.Content.ReadAsStringAsync();


                //Get Objects==============================================================
                JObject? jObject = JsonConvert.DeserializeObject<dynamic>(content);


                var success = jObject!.Value<bool>("success");
                var message = jObject.Value<string>("message");

                infoProfile = null!;
                var _infoUser = new InfoProfile
                {
                    Successful = success,
                    Message = message,
                };
                infoProfile = _infoUser;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                infoProfile = null!;
            }
            return infoProfile;

        }

        public async Task<EditAccount?> EditAccount(EditAccount editAccount)
        {
            LocalStorageModel localStorageModel = new LocalStorageModel();

            try
            {
                localStorageModel = await _localStorage.GetItemAsync<LocalStorageModel>("Storage");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                     "Bearer", localStorageModel.Token);


                //var model = new InfoProfile
                //{
                //    UserId = userid,
                //};

                var json = JsonConvert.SerializeObject(editAccount);
                HttpContent httpContent = new StringContent(json);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await _httpClient.PostAsync(CMSConstant.BaseApiAddress + "api/Profile/EditAccount", httpContent);

                var content = await response.Content.ReadAsStringAsync();


                //Get Objects==============================================================
                JObject? jObject = JsonConvert.DeserializeObject<dynamic>(content);


                var success = jObject!.Value<bool>("success");
                var message = jObject.Value<string>("message");

                editAccount = null!;
                var _editAccount = new EditAccount
                {
                    Successful = success,
                    Message = message,
                };
                editAccount = _editAccount;



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                editAccount = null!;
            }
            return editAccount;
        }
    }
}

