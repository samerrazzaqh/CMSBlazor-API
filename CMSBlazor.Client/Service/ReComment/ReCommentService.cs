using System;
using System.Net.Http.Headers;
using Blazored.LocalStorage;
using CMSBlazor.Client.Helpers;
using CMSBlazor.Client.Models;
using CMSBlazor.Shared.ViewModels.CommentLike;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CMSBlazor.Client.Service.ReComment
{
    public class ReCommentService : IReCommentService
	{
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;

        public ReCommentService(HttpClient httpClient,
           AuthenticationStateProvider authenticationStateProvider,
           ILocalStorageService _localStorage)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            this._localStorage = _localStorage;
        }

     

        public async Task<ReComentsViewModel> ListReComment(ReComentsViewModel model)
        {
            LocalStorageModel localStorageModel = new LocalStorageModel();
            ReComentsViewModel reComentsViewModel = null!;
            try
            {
                localStorageModel = await _localStorage.GetItemAsync<LocalStorageModel>("Storage");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                     "Bearer", localStorageModel.Token);

                var json = JsonConvert.SerializeObject(model);
                HttpContent httpContent = new StringContent(json);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                var response = await _httpClient.PostAsync(CMSConstant.BaseApiAddress + "api/ReComments/ListReComment", httpContent);
                var content = await response.Content.ReadAsStringAsync();




                //Get Objects==============================================================
                JObject? jObject = JsonConvert.DeserializeObject<dynamic>(content);
                var success = jObject!.Value<bool>("success");
                var numberRecomment = jObject.Value<int>("numberRecomment");
                JToken recomments = jObject["recomments"]!;
                var InfoReComment = JsonConvert.DeserializeObject<List<InfoReComment>>(recomments.ToString());



                var _reComentsViewModel = new ReComentsViewModel
                {
                    Successful = success,
                    numberRecomment= numberRecomment,
                    InfoReComments = InfoReComment
                };
                reComentsViewModel = _reComentsViewModel;



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                reComentsViewModel = null!;
            }
            return reComentsViewModel;
        }


        public async Task<ReComentsViewModel> ListLikeReComment(ReComentsViewModel model)
        {
            LocalStorageModel localStorageModel = new LocalStorageModel();
            ReComentsViewModel reComentsViewModel = null!;
            try
            {
                localStorageModel = await _localStorage.GetItemAsync<LocalStorageModel>("Storage");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                     "Bearer", localStorageModel.Token);

                var json = JsonConvert.SerializeObject(model);
                HttpContent httpContent = new StringContent(json);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                var response = await _httpClient.PostAsync(CMSConstant.BaseApiAddress + "api/ReComments/ListLikeReComment", httpContent);
                var content = await response.Content.ReadAsStringAsync();




                //Get Objects==============================================================
                JObject? jObject = JsonConvert.DeserializeObject<dynamic>(content);
                var success = jObject!.Value<bool>("success");
                var numberlikesReComment = jObject!.Value<int>("numberlikesReComment");
                JToken getLikeReComments = jObject["getLikeReComments"]!;
                var LikeReComments = JsonConvert.DeserializeObject<List<LikeReComments>>(getLikeReComments.ToString());



                var _reComentsViewModel = new ReComentsViewModel
                {
                    Successful = success,
                    LikeReComments = LikeReComments,
                    numberlikesReComment = numberlikesReComment
                };
                reComentsViewModel = _reComentsViewModel;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                reComentsViewModel = null!;
            }
            return reComentsViewModel;
        }



        public async Task<ReComentsViewModel> CreateReComment(ReComentsViewModel model)
        {
            LocalStorageModel localStorageModel = new LocalStorageModel();
            ReComentsViewModel reComentsViewModel = null!;
            try
            {
                localStorageModel = await _localStorage.GetItemAsync<LocalStorageModel>("Storage");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                     "Bearer", localStorageModel.Token);

                var json = JsonConvert.SerializeObject(model);
                HttpContent httpContent = new StringContent(json);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                var response = await _httpClient.PostAsync(CMSConstant.BaseApiAddress + "api/ReComments/CreateReComment", httpContent);
                var content = await response.Content.ReadAsStringAsync();




                //Get Objects==============================================================
                JObject? jObject = JsonConvert.DeserializeObject<dynamic>(content);
                var success = jObject!.Value<bool>("success");



                var _reComentsViewModel = new ReComentsViewModel
                {
                    Successful = success,
                };
                reComentsViewModel = _reComentsViewModel;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                reComentsViewModel = null!;
            }
            return reComentsViewModel;
        }

        public async Task<ReComentsViewModel> DeleteReComment(ReComentsViewModel model)
        {
            LocalStorageModel localStorageModel = new LocalStorageModel();
            ReComentsViewModel reComentsViewModel = null!;
            try
            {
                localStorageModel = await _localStorage.GetItemAsync<LocalStorageModel>("Storage");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                     "Bearer", localStorageModel.Token);

                var json = JsonConvert.SerializeObject(model);
                HttpContent httpContent = new StringContent(json);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                var response = await _httpClient.PostAsync(CMSConstant.BaseApiAddress + "api/ReComments/DeleteReComment", httpContent);
                var content = await response.Content.ReadAsStringAsync();




                //Get Objects==============================================================
                JObject? jObject = JsonConvert.DeserializeObject<dynamic>(content);
                var success = jObject!.Value<bool>("success");



                var _reComentsViewModel = new ReComentsViewModel
                {
                    Successful = success,
                };
                reComentsViewModel = _reComentsViewModel;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                reComentsViewModel = null!;
            }
            return reComentsViewModel;
        }

        public async Task<ReComentsViewModel> EditReComment(ReComentsViewModel model)
        {
            LocalStorageModel localStorageModel = new LocalStorageModel();
            ReComentsViewModel reComentsViewModel = null!;
            try
            {
                localStorageModel = await _localStorage.GetItemAsync<LocalStorageModel>("Storage");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                     "Bearer", localStorageModel.Token);

                var json = JsonConvert.SerializeObject(model);
                HttpContent httpContent = new StringContent(json);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                var response = await _httpClient.PostAsync(CMSConstant.BaseApiAddress + "api/ReComments/EditReComment", httpContent);
                var content = await response.Content.ReadAsStringAsync();




                //Get Objects==============================================================
                JObject? jObject = JsonConvert.DeserializeObject<dynamic>(content);
                var success = jObject!.Value<bool>("success");


                var _reComentsViewModel = new ReComentsViewModel
                {
                    Successful = success,
                };
                reComentsViewModel = _reComentsViewModel;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                reComentsViewModel = null!;
            }
            return reComentsViewModel;
        }

        public async Task<ReComentsViewModel> LikeReCommentCreate(ReComentsViewModel model)
        {
            LocalStorageModel localStorageModel = new LocalStorageModel();
            ReComentsViewModel reComentsViewModel = null!;
            try
            {
                localStorageModel = await _localStorage.GetItemAsync<LocalStorageModel>("Storage");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                     "Bearer", localStorageModel.Token);

                var json = JsonConvert.SerializeObject(model);
                HttpContent httpContent = new StringContent(json);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                var response = await _httpClient.PostAsync(CMSConstant.BaseApiAddress + "api/ReComments/LikeReCommentCreate", httpContent);
                var content = await response.Content.ReadAsStringAsync();




                //Get Objects==============================================================
                JObject? jObject = JsonConvert.DeserializeObject<dynamic>(content);
                var success = jObject!.Value<bool>("success");


                var _reComentsViewModel = new ReComentsViewModel
                {
                    Successful = success,
                };
                reComentsViewModel = _reComentsViewModel;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                reComentsViewModel = null!;
            }
            return reComentsViewModel;
        }
    }
}

