using System;
using CMSBlazor.Data;
using CMSBlazor.Shared.ViewModels.CommentLike;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CMSBlazor.Shared.Models;

namespace CMSBlazor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ReCommentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReCommentsController(AppDbContext context)
        {
            _context = context;
        }



        [HttpPost("ListReComment")]
        public async Task<ActionResult> ListReComment(ReComentsViewModel model)
        {
            int num = model.GetNumReComment;

            var numberRecomment = _context.ReComments.Where(x => x.PostId == model.PostId && x.CommentId == model.CommentId).Count();

            var getReComments = (from c in _context.ReComments.OrderByDescending(p => p.LCODate)
                                 where (c.PostId == model.PostId && c.CommentId == model.CommentId)
                                 select new
                                 {
                                     PostId = c.PostId,
                                     CommentId = c.CommentId,
                                     ReCommentId = c.ReCommentId,
                                     ReTextComment = c.ReTextComment,
                                     LCODate = c.LCODate,
                                     UserId = c.AboutUsers!.ApplicationUsers!.Id,
                                     Auther = c.AboutUsers!.ApplicationUsers!.UserName,
                                     UrlImageAuther = $"/profile/{c.AboutUsers.UrlImageProfile}",
                                     numberlikesReComment = _context.LikeReComments.Where(x => x.CommentId == c.CommentId && x.ReCommentId == c.ReCommentId)
                                     .Where(x => x.PostId == c.PostId).Count().ToString(),
                                 }).Take(num).ToListAsync();

            return Ok(new { success = true, recomments = await getReComments, numberRecomment = numberRecomment });

        }


        



        [HttpPost("CreateReComment")]
        public async Task<ActionResult> CreateReComment(ReComentsViewModel model)
        {
            ReComment recomment = new ReComment();
            if (ModelState.IsValid)
            {
                var aboutid = _context.AboutUsers.Where(e => e.ApplicationUsers!.Id == model.UserId)
               .Select(e => e.AboutUserId).FirstOrDefault();

                var username = _context.Users.Where(e => e.Id == model.UserId)
               .Select(e => e.UserName).FirstOrDefault();

                if (aboutid == null)
                {
                    return Ok(new { success = false, message = "AboutUser Null" });
                }
                recomment.ReTextComment = model.ReTextComment;
                recomment.LCODate = DateTime.Now;
                recomment.AboutUserId = aboutid;
                recomment.PostId = model.PostId;
                recomment.CommentId = model.CommentId;
                _context.Add(recomment);
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = $"Add ReCommen from {username}" });
            }
            return Ok(new { success = false, message = "ModelState Error" });
        }




        [HttpPost("EditReComment")]
        public async Task<IActionResult> EditReComment(ReComentsViewModel model)
        {
            try
            {
                var aboutid = _context.AboutUsers.Where(e => e.ApplicationUsers!.Id == model.UserId)
             .Select(e => e.AboutUserId).FirstOrDefault();

                var username = _context.Users.Where(e => e.Id == model.UserId)
                 .Select(e => e.UserName).FirstOrDefault();

                if (aboutid == null)
                {
                    return Ok(new { success = false, message = "AboutUser Null" });
                }


                var _ReComments = await _context.ReComments.FindAsync(model.ReCommentId);
                if (_ReComments != null)
                {
                    _ReComments.ReTextComment = model.ReTextComment;
                    _context.ReComments.Attach(_ReComments);
                    _context.Entry(_ReComments).Property(x => x.ReTextComment).IsModified = true;
                    await _context.SaveChangesAsync();
                    return Ok(new { success = true, message = "Edit this Comment" });
                }
            }

            catch (Exception ex)
            {
                return Ok(new { exception = ex.Message.ToString() });
            }

            return Ok(new { success = false, message = "Error Edit" });
        }






        [HttpPost("DeleteReComment")]
        public async Task<ActionResult> DeleteReComment(ReComentsViewModel model)
        {
            var aboutid = _context.AboutUsers.Where(e => e.ApplicationUsers!.Id == model.UserId)
             .Select(e => e.AboutUserId).FirstOrDefault();

            var username = _context.Users.Where(e => e.Id == model.UserId)
               .Select(e => e.UserName).FirstOrDefault();

            var recomment = await _context.ReComments.FindAsync(model.ReCommentId);
            _context.ReComments.Remove(recomment!);
            await _context.SaveChangesAsync();
            return Ok(new { success = true, message = "Remove this ReComment " + model.ReCommentId + " By " + username });
        }


        [HttpPost("ListLikeReComment")]
        public async Task<ActionResult> ListLikeReComment(ReComentsViewModel model)
        {
            int num = model.GetNumReComment;

            var NumberlikesReComment = _context.LikeReComments.Where(x => x.CommentId == model.CommentId &&
                                                                     x.ReCommentId == model.ReCommentId &&
                                                                     x.PostId == model.PostId).Count();

            var getLikeReComments = (from c in _context.LikeReComments
                                     where (c.PostId == model.PostId && c.CommentId == model.CommentId && c.ReCommentId == model.ReCommentId)
                                     select new
                                     {
                                         UserId = c.AboutUsers!.ApplicationUsers!.Id,
                                         Auther = c.AboutUsers!.ApplicationUsers!.UserName,
                                         UrlImageAuther = $"/profile/{c.AboutUsers.UrlImageProfile}",
                                     }).ToListAsync();


            return Ok(new { success = true, getLikeReComments = await getLikeReComments, numberlikesReComment = NumberlikesReComment });

        }







        [HttpPost("LikeReCommentCreate")]
        public async Task<ActionResult> LikeReCommentCreate( ReComentsViewModel model)
        {
            LikeReComment likereComment = new LikeReComment();
            var aboutid = _context.AboutUsers.Where(e => e.ApplicationUsers!.Id == model.UserId)
                .Select(e => e.AboutUserId).FirstOrDefault();

            if (aboutid == null)
            {
                return Ok(new { success = false, message = "AboutUser Null" });
            }

            var _LikeReComment = _context.LikeReComments
                .Where(e => e.PostId == model.PostId && e.CommentId == model.CommentId)
                .Where(x => x.AboutUserId == aboutid && x.ReCommentId == model.ReCommentId).FirstOrDefault();
            if (_LikeReComment == null)
            {
                likereComment.LikReComment = 1;
                likereComment.PostId = model.PostId;
                likereComment.CommentId = model.CommentId;
                likereComment.ReCommentId = model.ReCommentId;
                likereComment.AboutUserId = aboutid;
                _context.Add(likereComment);
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Add Like to this comment" });
            }
            else
            {
                var removelike = await _context.LikeReComments.FindAsync(_LikeReComment.LikeReCommentId);
                _context.LikeReComments.Remove(removelike!);
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Remove Like from comment" });
            }

        }



    }
}

