using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Blazored.LocalStorage;
using CMSBlazor.Client.Helpers;
using CMSBlazor.Client.Models;
using CMSBlazor.Shared.ViewModels.CommentLike;
using CMSBlazor.Shared.ViewModels.Post;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CMSBlazor.Client.Service.Comment
{
	public class CommentService : ICommentService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;

        public CommentService(HttpClient httpClient,
            AuthenticationStateProvider authenticationStateProvider,
            ILocalStorageService _localStorage)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            this._localStorage = _localStorage;
        }

    

        public async Task<CommentsViewModel> ListComment(CommentsViewModel model)
        {
            LocalStorageModel localStorageModel = new LocalStorageModel();
            CommentsViewModel commentsViewModel = null!;
            try
            {
                localStorageModel = await _localStorage.GetItemAsync<LocalStorageModel>("Storage");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                     "Bearer", localStorageModel.Token);

                var json = JsonConvert.SerializeObject(model);
                HttpContent httpContent = new StringContent(json);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                var response = await _httpClient.PostAsync(CMSConstant.BaseApiAddress + "api/Comments/ListComment", httpContent);
                var content = await response.Content.ReadAsStringAsync();



                
                //Get Objects==============================================================
                JObject? jObject = JsonConvert.DeserializeObject<dynamic>(content);
                var success = jObject!.Value<bool>("success");
                var postscount = jObject.Value<int>("commentsCount");
                JToken comments = jObject["comments"]!;
                var InfoComment = JsonConvert.DeserializeObject<List<InfoComment>>(comments.ToString());



                var _commentsViewModel = new CommentsViewModel
                {
                    Successful = success,

                    PostsCount = postscount,
                    InfoComments = InfoComment
                };
                commentsViewModel = _commentsViewModel;



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                commentsViewModel = null!;
            }
            return commentsViewModel;
        }


        public async Task<CommentsViewModel> CreateComment(CommentsViewModel model)
        {
            LocalStorageModel localStorageModel = new LocalStorageModel();
            CommentsViewModel commentsViewModel = null!;
            try
            {
                localStorageModel = await _localStorage.GetItemAsync<LocalStorageModel>("Storage");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                     "Bearer", localStorageModel.Token);

                var json = JsonConvert.SerializeObject(model);
                HttpContent httpContent = new StringContent(json);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                var response = await _httpClient.PostAsync(CMSConstant.BaseApiAddress + "api/Comments/CreateComment", httpContent);
                var content = await response.Content.ReadAsStringAsync();

                //Get Objects==============================================================
                JObject? jObject = JsonConvert.DeserializeObject<dynamic>(content);
                var success = jObject!.Value<bool>("success");

                var _commentsViewModel = new CommentsViewModel
                {
                    Successful = success,
                };
                commentsViewModel = _commentsViewModel;



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                commentsViewModel = null!;
            }
            return commentsViewModel;
        }

        public async Task<CommentsViewModel> DeleteComment(CommentsViewModel model)
        {
            LocalStorageModel localStorageModel = new LocalStorageModel();
            CommentsViewModel commentsViewModel = null!;
            try
            {
                localStorageModel = await _localStorage.GetItemAsync<LocalStorageModel>("Storage");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                     "Bearer", localStorageModel.Token);

                var json = JsonConvert.SerializeObject(model);
                HttpContent httpContent = new StringContent(json);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                var response = await _httpClient.PostAsync(CMSConstant.BaseApiAddress + "api/Comments/DeleteComment", httpContent);
                var content = await response.Content.ReadAsStringAsync();

                //Get Objects==============================================================
                JObject? jObject = JsonConvert.DeserializeObject<dynamic>(content);
                var success = jObject!.Value<bool>("success");

                var _commentsViewModel = new CommentsViewModel
                {
                    Successful = success,
                };
                commentsViewModel = _commentsViewModel;



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                commentsViewModel = null!;
            }
            return commentsViewModel;
        }

        public async Task<CommentsViewModel> EditComment(CommentsViewModel model)
        {
            LocalStorageModel localStorageModel = new LocalStorageModel();
            CommentsViewModel commentsViewModel = null!;
            try
            {
                localStorageModel = await _localStorage.GetItemAsync<LocalStorageModel>("Storage");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                     "Bearer", localStorageModel.Token);

                var json = JsonConvert.SerializeObject(model);
                HttpContent httpContent = new StringContent(json);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                var response = await _httpClient.PostAsync(CMSConstant.BaseApiAddress + "api/Comments/EditComment", httpContent);
                var content = await response.Content.ReadAsStringAsync();

                //Get Objects==============================================================
                JObject? jObject = JsonConvert.DeserializeObject<dynamic>(content);
                var success = jObject!.Value<bool>("success");

                var _commentsViewModel = new CommentsViewModel
                {
                    Successful = success,
                };
                commentsViewModel = _commentsViewModel;



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                commentsViewModel = null!;
            }
            return commentsViewModel;
        }

        public async Task<CommentsViewModel> LikeCommentCreate(LikesComment model)
        {
            LocalStorageModel localStorageModel = new LocalStorageModel();
            CommentsViewModel commentsViewModel = null!;
            try
            {
                localStorageModel = await _localStorage.GetItemAsync<LocalStorageModel>("Storage");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                     "Bearer", localStorageModel.Token);

                var json = JsonConvert.SerializeObject(model);
                HttpContent httpContent = new StringContent(json);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                var response = await _httpClient.PostAsync(CMSConstant.BaseApiAddress + "api/Comments/LikeCommentCreate", httpContent);
                var content = await response.Content.ReadAsStringAsync();

                //Get Objects==============================================================
                JObject? jObject = JsonConvert.DeserializeObject<dynamic>(content);
                var success = jObject!.Value<bool>("success");

                var _commentsViewModel = new CommentsViewModel
                {
                    Successful = success,
                };
                commentsViewModel = _commentsViewModel;



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                commentsViewModel = null!;
            }
            return commentsViewModel;
        }



        public async Task<PostViewModel> LikePostCreate(CommentsViewModel model)
        {
            LocalStorageModel localStorageModel = new LocalStorageModel();
            PostViewModel postViewModels = null!;
            try
            {
                localStorageModel = await _localStorage.GetItemAsync<LocalStorageModel>("Storage");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                     "Bearer", localStorageModel.Token);



                var json = JsonConvert.SerializeObject(model);
                HttpContent httpContent = new StringContent(json);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                var response = await _httpClient.PostAsync(CMSConstant.BaseApiAddress + "api/Comments/LikePostCreate", httpContent);
                var content = await response.Content.ReadAsStringAsync();

                //Get Objects==============================================================
                JObject? jObject = JsonConvert.DeserializeObject<dynamic>(content);
                var success = jObject!.Value<bool>("success");
                var post = jObject.Value<object>("post");
                JObject? jObject_post = JsonConvert.DeserializeObject<dynamic>(post!.ToString()!);
                var postId = jObject_post!.Value<long>("postId");
                var title = jObject_post!.Value<string>("title");
                var postImg = jObject_post!.Value<string>("postImg");
                var postContent = jObject_post!.Value<string>("postContent");
                var linkVideo = jObject_post!.Value<string>("linkVideo");
                var postDate = jObject_post.Value<DateTime>("postDate");
                var categoryId = jObject_post!.Value<int>("categoryId");
                var catName = jObject_post!.Value<string>("catName");
                var postViews = jObject_post!.Value<int>("postViews");
                var auther = jObject_post!.Value<string>("auther");
                var urlImageAuther = jObject_post!.Value<string>("urlImageAuther");
                var numbercomment = jObject_post!.Value<int>("numbercomment");
                var numberlikes = jObject_post!.Value<int>("numberlikes");
                JToken usersLikePost = jObject_post["usersLikePost"]!;
                var usersLikePostlist = JsonConvert.DeserializeObject<List<UsersLikePost>>(usersLikePost.ToString());


                var _postViewModels = new PostViewModel
                {
                    Successful = success,

                    PostId = postId,
                    Title = title,
                    PostImg = postImg,
                    PostContent = postContent,
                    LinkVideo = linkVideo,
                    PostDate = postDate,
                    CategoryId = categoryId,
                    CatName = catName,
                    PostViews = postViews,
                    Auther = auther,
                    UrlImageAuther = urlImageAuther,
                    NumberComment = numbercomment,
                    NumberLikes = numberlikes,
                    UsersLikePosts = usersLikePostlist

                };
                postViewModels = _postViewModels;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                postViewModels = null!;
            }
            return postViewModels;
        }



    }
}

