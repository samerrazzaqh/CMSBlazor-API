using System;
using CMSBlazor.Shared.ViewModels.CommentLike;

namespace CMSBlazor.Client.Service.ReComment
{
	public interface IReCommentService
	{
        Task<ReComentsViewModel> ListReComment(ReComentsViewModel model);

        Task<ReComentsViewModel> CreateReComment(ReComentsViewModel model);

        Task<ReComentsViewModel> DeleteReComment(ReComentsViewModel model);

        Task<ReComentsViewModel> EditReComment(ReComentsViewModel model);

        Task<ReComentsViewModel> ListLikeReComment(ReComentsViewModel model);

        Task<ReComentsViewModel> LikeReCommentCreate(ReComentsViewModel model);


    }
}

