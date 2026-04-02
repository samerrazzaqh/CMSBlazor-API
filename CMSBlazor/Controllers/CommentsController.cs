using System;
using CMSBlazor.Data;
using CMSBlazor.Shared.Models;
using CMSBlazor.Shared.ViewModels.CommentLike;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMSBlazor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class CommentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CommentsController(AppDbContext context)
        {
            _context = context;
        }


        [HttpPost("ListComment")]
        public async Task<ActionResult> ListComment(CommentsViewModel commentsViewModel)
        {
            var commentsCount = _context.Comments.Where(x =>x.PostId == commentsViewModel.PostId).Count().ToString();
            int num = commentsViewModel.GetNumComment;

            var getComments = (from c in _context.Comments.OrderByDescending(p => p.LCODate)
                               where (c.PostId == commentsViewModel.PostId)
                               select new
                               {
                                   PostId = c.PostId,
                                   CommentId = c.CommentId,
                                   TextComment = c.TextComment,
                                   LCODate = c.LCODate,
                                   LikComment = _context.LikeComments.Where(x => x.PostId == c.PostId && x.CommentId == c.CommentId).Select(x=>x.LikComment).FirstOrDefault(),
                                   UserId = c.AboutUsers!.ApplicationUsers!.Id,
                                   Auther = c.AboutUsers!.ApplicationUsers!.UserName,
                                   UrlImageAuther = $"/profile/{c.AboutUsers.UrlImageProfile}",
                                   numberRecomment = _context.ReComments.Where(x => x.PostId == c.PostId && x.CommentId == c.CommentId).Count().ToString(),
                                   numberlikesComment = _context.LikeComments.Where(x => x.PostId == c.PostId && x.CommentId == c.CommentId).Count().ToString(),
                                   UsersLikeComment = (from u in _context.LikeComments
                                                       where(u.PostId == c.PostId
                                                       && u.CommentId == c.CommentId)
                                                       select new
                                                       {
                                                           UserName = u.AboutUsers!.ApplicationUsers!.UserName,
                                                           UrlImageProfile = $"/profile/{u.AboutUsers!.UrlImageProfile}",
                                                           UserId = u.AboutUsers!.ApplicationUsers!.Id,
                                                       }).ToList(),

                               }).Take(num).ToListAsync();

            return Ok(new { success = true, comments = await getComments, commentsCount = commentsCount });

        }



        [HttpPost("CreateComment")]
        public async Task<ActionResult> CreateComment(CommentsViewModel commentsViewModel)
        {
            if (ModelState.IsValid)
            {
                var aboutid = _context.AboutUsers.Where(e => e.ApplicationUsers!.Id == commentsViewModel.UserId)
               .Select(e => e.AboutUserId).FirstOrDefault();

                var username = _context.Users.Where(e => e.Id == commentsViewModel.UserId)
               .Select(e => e.UserName).FirstOrDefault();

                if (aboutid == null)
                {
                    return Ok(new { success = false, message = "aboutid null" });
                }

                var comment = new Comment
                {
                    TextComment = commentsViewModel.TextComment,
                    LCODate = DateTime.Now,
                    AboutUserId = aboutid,
                    PostId = commentsViewModel.PostId,

                };
                await _context.Comments.AddAsync(comment);
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Add Comment" });
            }
            return Ok(new { success = false, message = "Error" });
        }



        [HttpPost("EditComment")]
        public async Task<ActionResult> EditComment(CommentsViewModel commentsViewModel)
        {
            string message = "";
            bool success = false;

            try
            {
                var aboutid = _context.AboutUsers.Where(e => e.ApplicationUsers!.Id == commentsViewModel.UserId)
             .Select(e => e.AboutUserId).FirstOrDefault();

                var username = _context.Users.Where(e => e.Id == commentsViewModel.UserId)
                 .Select(e => e.UserName).FirstOrDefault();

                if (aboutid == null)
                {
                    message = "AboutUser Null";
                    success = false;
                }


                var _Comments = await _context.Comments.FindAsync(commentsViewModel.CommentId);
                if (_Comments != null)
                {
                    _Comments.TextComment = commentsViewModel.TextComment;

                    _context.Comments.Attach(_Comments);
                    _context.Entry(_Comments).Property(x => x.TextComment).IsModified = true;
                    await _context.SaveChangesAsync();

                    message = "Edit this Comment " + _Comments.CommentId + " By " + username;
                    success = true;
                }
            }

            catch (Exception ex)
            {
                return Ok(new { exception = ex.Message.ToString() });
            }

            return Ok(new { success = success, message = message });
        }




        [HttpPost("DeleteComment")]
        public async Task<ActionResult> DeleteComment(CommentsViewModel commentsViewModel)
        {
            string message = "";
            bool success = false;

            var aboutid = _context.AboutUsers.Where(e => e.ApplicationUsers!.Id == commentsViewModel.UserId)
            .Select(e => e.AboutUserId).FirstOrDefault();

            var username = _context.Users.Where(e => e.Id == commentsViewModel.UserId)
               .Select(e => e.UserName).FirstOrDefault();
            if (aboutid == null)
            {
                return Ok(new { success = false, message = "AboutUser Null" });
            }

            var comment = await _context.Comments.FindAsync(commentsViewModel.CommentId);
            if (comment == null)
            {
                return Ok(new { success = false, message = "commentId Null" });
            }

            _context.Comments.Remove(comment!);
            await _context.SaveChangesAsync();
            message = "Remove this Comment " + commentsViewModel.CommentId + " By " + username;
            success = true;
            return Ok(new { success = success, message = message });
        }



        [HttpPost("LikePostCreate")]
        public async Task<ActionResult> LikePostCreate(CommentsViewModel commentsViewModel)
        {
            LikePost likePost = new LikePost();
            var aboutid = _context.AboutUsers.Where(e => e.ApplicationUsers!.Id == commentsViewModel.UserId)
                .Select(e => e.AboutUserId).FirstOrDefault();

            if (aboutid == null)
            {
                return Ok(new { success = false, message = "AboutUser Null" });
            }

            var _LikePosts = _context.LikePosts.Where(e => e.PostId == commentsViewModel.PostId && e.AboutUserId == aboutid).FirstOrDefault();
            if (_LikePosts == null)
            {
                likePost.LikPost = 1;
                likePost.PostId = commentsViewModel.PostId;
                likePost.AboutUserId = aboutid;
                _context.Add(likePost);
                await _context.SaveChangesAsync();


                var getPost = (from c in _context.Posts
                               where (c.PostId == commentsViewModel.PostId)
                               select new
                               {
                                   PostId = c.PostId,
                                   Title = c.Title,
                                   PostImg = $"/post/{c.PostImg}",
                                   PostContent = c.PostContent,
                                   LinkVideo = c.LinkVideo,
                                   PostDate = c.PostDate,
                                   CategoryId = c.Categories!.CategoryId,
                                   CatName = c.Categories!.CatName,
                                   PostViews = c.PostViews,
                                   Auther = c.AboutUsers!.ApplicationUsers!.UserName,
                                   UrlImageAuther = $"/profile/{c.AboutUsers.UrlImageProfile}",
                                   numbercomment = _context.Comments.Where(x => x.PostId == c.PostId).Count().ToString(),
                                   numberlikes = _context.LikePosts.Where(x => x.PostId == c.PostId).Count().ToString(),
                                   UsersLikePost = (from p in _context.LikePosts
                                                    where (p.PostId == c.PostId)
                                                    select new
                                                    {
                                                        UserName = p.AboutUsers!.ApplicationUsers!.UserName,
                                                        UrlImageProfile = $"/profile/{p.AboutUsers!.UrlImageProfile}",
                                                        UserId = p.AboutUsers!.ApplicationUsers!.Id,
                                                    }).ToList(),
                               }).FirstOrDefaultAsync();

                return Ok(new { success = true, post = await getPost , message = "Add Like" });
            }
            else
            {
                var removelike = await _context.LikePosts.FindAsync(_LikePosts.LikePostId);
                _context.LikePosts.Remove(removelike!);
                await _context.SaveChangesAsync();


                var getPost = (from c in _context.Posts
                               where (c.PostId == commentsViewModel.PostId)
                               select new
                               {
                                   PostId = c.PostId,
                                   Title = c.Title,
                                   PostImg = $"/post/{c.PostImg}",
                                   PostContent = c.PostContent,
                                   LinkVideo = c.LinkVideo,
                                   PostDate = c.PostDate,
                                   CategoryId = c.Categories!.CategoryId,
                                   CatName = c.Categories!.CatName,
                                   PostViews = c.PostViews,
                                   Auther = c.AboutUsers!.ApplicationUsers!.UserName,
                                   UrlImageAuther = $"/profile/{c.AboutUsers.UrlImageProfile}",
                                   numbercomment = _context.Comments.Where(x => x.PostId == c.PostId).Count().ToString(),
                                   numberlikes = _context.LikePosts.Where(x => x.PostId == c.PostId).Count().ToString(),
                                   UsersLikePost = (from p in _context.LikePosts
                                                    where (p.PostId == c.PostId)
                                                    select new
                                                    {
                                                        UserName = p.AboutUsers!.ApplicationUsers!.UserName,
                                                        UrlImageProfile = $"/profile/{p.AboutUsers!.UrlImageProfile}",
                                                        UserId = p.AboutUsers!.ApplicationUsers!.Id,
                                                    }).ToList(),
                               }).FirstOrDefaultAsync();
                return Ok(new { success = true, post = await getPost, message = "Remove Like" });
            }

        }




        [HttpPost("LikeCommentCreate")]
        public async Task<ActionResult> LikeCommentCreate(LikesComment likesComment)
        {
            LikeComment likeComment = new LikeComment();
            var aboutid = _context.AboutUsers.Where(e => e.ApplicationUsers!.Id == likesComment.UserId)
                .Select(e => e.AboutUserId).FirstOrDefault();

            if (aboutid == null)
            {
                return Ok(new { success = false, message = "AboutUser Null" });
            }

            var _LikeComment = _context.LikeComments
                .Where(e => e.PostId == likesComment.PostId && e.CommentId == likesComment.CommentId)
                .Where(x => x.AboutUserId == aboutid).FirstOrDefault();
            if (_LikeComment == null)
            {
                likeComment.LikComment = 1;
                likeComment.PostId = likesComment.PostId;
                likeComment.CommentId = likesComment.CommentId;
                likeComment.AboutUserId = aboutid;
                _context.Add(likeComment);
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Add Like to this comment : " });
            }
            else
            {
                var removelike = await _context.LikeComments.FindAsync(_LikeComment.LikeCommentId);
                _context.LikeComments.Remove(removelike!);
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Remove Like to this comment : " });
            }

        }

      


    }
}

