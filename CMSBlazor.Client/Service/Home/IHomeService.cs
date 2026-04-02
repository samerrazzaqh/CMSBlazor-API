using System;
using CMSBlazor.Shared.ViewModels.Post;

namespace CMSBlazor.Client.Service.Home
{
	public interface IHomeService
	{
        Task<IEnumerable<PostViewModel>?> ListPost(int numpost);

        Task<IEnumerable<PostViewModel>?> MostPopularPost(int numpost);

        Task<IEnumerable<PostViewModel>?> ListPostByCategory(int numpost, int catId);

        Task<PostViewModel> SinglePost(PostUtilities postUtilities);

        //Task<PostUtilities> DeletePost(PostUtilities postUtilities);

        //Task<PostViewModel?> GetEditPost(PostUtilities postUtilities);
        //Task<PostViewModel?> EditPost(PostViewModel postViewModel);
    }



}

