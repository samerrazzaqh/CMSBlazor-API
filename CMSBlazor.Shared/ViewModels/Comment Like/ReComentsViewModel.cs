using System;
using CMSBlazor.Shared.Models;

namespace CMSBlazor.Shared.ViewModels.CommentLike
{
	public class ReComentsViewModel
	{
        public ReComentsViewModel()
        {
            InfoReComments = new List<InfoReComment>();
            LikeReComments = new List<LikeReComments>();
        }



        public string? UserId { get; set; }
        public int GetNumReComment { get; set; }

        public long CommentId { get; set; }
        public long PostId { get; set; }
        public string? ReTextComment { get; set; }
        public long ReCommentId { get; set; }
        public int numberlikesReComment { get; set; }
        public int numberRecomment { get; set; }

        public List<InfoReComment>? InfoReComments { get; set; }
        public List<LikeReComments>? LikeReComments { get; set; }



        public string? Message { get; set; }
        public bool Successful { get; set; }
    }

    public class LikeReComments
    {
        public string? UserId { get; set; }
        public string? Auther { get; set; }
        public string? UrlImageAuther { get; set; }

        public long CommentId { get; set; }
        public long ReCommentId { get; set; }
        public long PostId { get; set; }
    }


    public class InfoReComment
    {
        public long PostId { get; set; }
        public long CommentId { get; set; }
        public long ReCommentId { get; set; }
        public string? ReTextComment { get; set; }
        public string? UserId { get; set; }
        public DateTime LCODate { get; set; }
        public string? Auther { get; set; }
        public string? UrlImageAuther { get; set; }
        public int numberlikesReComment { get; set; }
    }



}

