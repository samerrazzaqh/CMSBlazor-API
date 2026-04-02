using System;
using CMSBlazor.Shared.ViewModels.CommentLike;
using CMSBlazor.Shared.ViewModels.Post;

namespace CMSBlazor.Client.Service.Comment
{
	public interface ICommentService
	{
        Task<CommentsViewModel> ListComment(CommentsViewModel commentsViewModel);

        Task<CommentsViewModel> CreateComment(CommentsViewModel commentsViewModel);

        Task<CommentsViewModel> EditComment(CommentsViewModel commentsViewModel);

        Task<CommentsViewModel> DeleteComment(CommentsViewModel commentsViewModel);

        Task<PostViewModel> LikePostCreate(CommentsViewModel commentsViewModel);

        Task<CommentsViewModel> LikeCommentCreate(LikesComment likesComment);
    }
}

