using System;
using CMSBlazor.Shared.ViewModels.Administration;
using CMSBlazor.Shared.ViewModels.Post;

namespace CMSBlazor.Client.Service.Posts
{
	public interface IPostService
	{
        Task<PostAll> ListPost(int numpost);

        Task<PostViewModel> CreatePost(PostViewModel postViewModel);

        Task<PostUtilities> DeletePost(PostUtilities postUtilities);

        Task<PostViewModel?> GetEditPost(PostUtilities postUtilities);
        Task<PostViewModel?> EditPost(PostViewModel postViewModel);



    }
}

