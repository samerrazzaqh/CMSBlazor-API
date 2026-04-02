using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using CMSBlazor.Shared.Models;
using CMSBlazor.Shared.ViewModels.Post;

namespace CMSBlazor.Shared.ViewModels.CommentLike
{
	public class CommentsViewModel
	{

        public CommentsViewModel()
        {
            InfoComments = new List<InfoComment>();
            LikesComments = new List<LikesComment>();
        }

        

        public string? UserId { get; set; }
        public long CommentId { get; set; }
        public long PostId { get; set; }
        public string? TextComment { get; set; }
        public int GetNumComment { get; set; }

        public List<InfoComment>? InfoComments { get; set; }
        public List<LikesComment>? LikesComments { get; set; }
        public List<UsersLikePost>? UsersLikePosts { get; set; }

        public string? numberlikes { get; set; }

        public int PostsCount { get; set; }
        public int LikeCommentsCount { get; set; }



        public string? Message { get; set; }
        public bool Successful { get; set; }
    }

    public class InfoComment
    {
        public string? UserId { get; set; }
        public long CommentId { get; set; }
        public long PostId { get; set; }
        public string? TextComment { get; set; }
        public DateTime LCODate { get; set; }
        public string? Auther { get; set; }
        public string? UrlImageAuther { get; set; }
        public int LikComment { get; set; }
        public int NumberRecomment { get; set; }
        public int NumberlikesComment { get; set; }
        public List<LikesComment>? UsersLikeComment { get; set; }
        public List<ReComments>? ReComments { get; set; }
    }


    public class LikesComment
    {
        public string? UserId { get; set; }
        public string? userName { get; set; }
        public long CommentId { get; set; }
        public long PostId { get; set; }


        public long LikeCommentId { get; set; }
        public int LikComment { get; set; }
        public string? aboutUserId { get; set; }
        public string? UrlImageProfile { get; set; }
    }


    public class ReComments
    {
        public string? userId { get; set; }
        public string? userName { get; set; }
        public string? reTextComment { get; set; }
        public string? urlImageProfile { get; set; }
        public string? numberlikesComment { get; set; }
        public List<LikesReComments>? LikesReComments { get; set; }
    }

    public class LikesReComments
    {
        public string? userId { get; set; }
        public string? userName { get; set; }
        public string? urlImageProfile { get; set; }
    }


}

