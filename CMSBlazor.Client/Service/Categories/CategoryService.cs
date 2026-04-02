using System;
using Blazored.LocalStorage;
using CMSBlazor.Shared.ViewModels.Post;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using CMSBlazor.Client.Models;
using CMSBlazor.Client.Helpers;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;

namespace CMSBlazor.Client.Service.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;
        public CategoryService(HttpClient httpClient,
            AuthenticationStateProvider authenticationStateProvider,
            ILocalStorageService _localStorage)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            this._localStorage = _localStorage;
        }

        public async Task<IEnumerable<CategoryViewModel>?> GetCategories()
        {
            LocalStorageModel localStorageModel = new LocalStorageModel();
            List<CategoryViewModel> result_categories = null!;
            try
            {
                localStorageModel = await _localStorage.GetItemAsync<LocalStorageModel>("Storage");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                     "Bearer", localStorageModel.Token);

                var response = await _httpClient.GetAsync(CMSConstant.BaseApiAddress + "api/Category");


                var content = await response.Content.ReadAsStringAsync();
                JObject jObject = JObject.Parse(content);
                JToken jToken = jObject["categories"]!;

                var categories = JsonConvert.DeserializeObject<List<CategoryViewModel>>(jToken.ToString());
                result_categories = categories!;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                result_categories = null!;
            }
            return result_categories;
        }



        public async Task<CategoryViewModel> CreateCategory(CategoryViewModel newCategory)
        {
            LocalStorageModel localStorageModel = new LocalStorageModel();
            localStorageModel = await _localStorage.GetItemAsync<LocalStorageModel>("Storage");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                 "Bearer", localStorageModel.Token);
            var result = await _httpClient.PostAsJsonAsync(CMSConstant.BaseApiAddress + "api/Category", newCategory);

            var content = await result.Content.ReadAsStringAsync();
            JObject? jObject = JsonConvert.DeserializeObject<dynamic>(content);

            var success = jObject!.Value<bool>("success");

            if (success == true)
            {
                newCategory.Message = jObject.Value<string>("message");
                newCategory.Successful = success;
            }
            else
            {
                newCategory.Message = jObject.Value<string>("message");
                newCategory.Successful = success;
            }
            return newCategory;

        }

        public async Task DeleteCategory(int id)
        {
            LocalStorageModel localStorageModel = new LocalStorageModel();
            localStorageModel = await _localStorage.GetItemAsync<LocalStorageModel>("Storage");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                 "Bearer", localStorageModel.Token);

            await _httpClient.DeleteAsync(CMSConstant.BaseApiAddress + $"api/Category/{id}");
        }

        public async Task<CategoryViewModel?> GetCategory(int id)
        {
            LocalStorageModel localStorageModel = new LocalStorageModel();
            localStorageModel = await _localStorage.GetItemAsync<LocalStorageModel>("Storage");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                 "Bearer", localStorageModel.Token);
            return await _httpClient.GetFromJsonAsync<CategoryViewModel>(CMSConstant.BaseApiAddress + $"api/Category/{id}");
        }

        public async Task<CategoryViewModel?> UpdateCategory(CategoryViewModel updatedCategory)
        {
            LocalStorageModel localStorageModel = new LocalStorageModel();
            localStorageModel = await _localStorage.GetItemAsync<LocalStorageModel>("Storage");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                 "Bearer", localStorageModel.Token);

            var result = await _httpClient.PutAsJsonAsync<CategoryViewModel>(CMSConstant.BaseApiAddress + "api/Category", updatedCategory);


            var content = await result.Content.ReadAsStringAsync();
            JObject jObject = JsonConvert.DeserializeObject<dynamic>(content)!;

            var success = jObject.Value<bool>("success");

            if (success == true)
            {
                updatedCategory.Message = jObject.Value<string>("message");
                updatedCategory.Successful = success;
            }
            else
            {
                updatedCategory.Message = jObject.Value<string>("message");
                updatedCategory.Successful = success;
            }
            return updatedCategory;




        }
    }
}

