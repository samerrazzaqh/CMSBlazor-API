using System;
using CMSBlazor.Data;
using CMSBlazor.Shared.Models;
using CMSBlazor.Shared.ViewModels.Post;
using CMSBlazor.Shared.ViewModels.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMSBlazor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PostsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment env;
        public PostsController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            this.env = env;
        }


        [HttpPost("ListPost")]
        public async Task<ActionResult> ListPost(PostUtilities postUtilities)
        {

            var postscount = _context.Posts.Count().ToString();
            int num = postUtilities.NumGetPost;

            if (!string.IsNullOrEmpty(postUtilities.Search))
            {
                var Getposts = (from p in _context.Posts.OrderByDescending(p => p.PostId)
                        .Include(p => p.AboutUsers!.ApplicationUsers)
                        .Where(x => x.Title.Contains(postUtilities.Search) ||
                                    x.PostContent.Contains(postUtilities.Search))
                    select new
                    {
                        PostId = p.PostId,
                        Auther = p.AboutUsers!.ApplicationUsers!.UserName,
                        UrlImageAuther = p.AboutUsers.UrlImageCover,
                        Title = p.Title,
                        PostContent = p.PostContent,
                        PostImg = $"/post/{p.PostImg}",
                        PostViews = p.PostViews,
                        LinkVideo = p.LinkVideo,
                        IsPublish = p.IsPublish,
                        CatName = p.Categories!.CatName,
                        PostDate = p.PostDate,
                        numbercomment = _context.Comments.Where(x => x.PostId == p.PostId).Count().ToString(),
                        numberlikes = _context.LikePosts.Where(x => x.PostId == p.PostId).Count().ToString(),
                        UsersLike = _context.LikePosts.Where(x => x.PostId == p.PostId).ToList(),

                    }).Take(num).ToListAsync();

                return Ok(new { success = true, posts = await Getposts, postscount = postscount });
            }
            else
            {
                var Getposts = (from p in _context.Posts.OrderByDescending(p => p.PostId)
                        .Include(p => p.AboutUsers!.ApplicationUsers)
                    select new
                    {
                        PostId = p.PostId,
                        Auther = p.AboutUsers!.ApplicationUsers!.UserName,
                        UrlImageAuther = p.AboutUsers.UrlImageCover,
                        Title = p.Title,
                        PostContent = p.PostContent,
                        PostImg = $"/post/{p.PostImg}",
                        PostViews = p.PostViews,
                        LinkVideo = p.LinkVideo,
                        IsPublish = p.IsPublish,
                        CatName = p.Categories!.CatName,
                        PostDate = p.PostDate,
                        numbercomment = _context.Comments.Where(x => x.PostId == p.PostId).Count().ToString(),
                        numberlikes = _context.LikePosts.Where(x => x.PostId == p.PostId).Count().ToString(),
                        UsersLike = _context.LikePosts.Where(x => x.PostId == p.PostId).ToList(),

                    }).Take(num).ToListAsync();

                return Ok(new { success = true, posts = await Getposts, postscount = postscount });
                
            }
            
            
            
           
            //var Getposts = _context.Posts.OrderByDescending(p => p.PostDate).Include(p => p.AboutUsers!.ApplicationUsers).Include(p => p.Categories).ToListAsync();
            //return Ok(new { success = true, posts = await Getposts, message = "ListPost" });

        }


        [HttpPost("CreatePost")]
        public async Task<ActionResult<PostViewModel>> CreatePost(PostViewModel postViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (postViewModel == null)
                    {
                        return Ok(new { success = false, message = "postViewModel null" });
                    }

                    string nameImgPost = string.Empty;
                    if (postViewModel.PostImg != null)
                    {
                        string extension = Path.GetExtension(postViewModel!.PostImgName!);
                        nameImgPost = $"{postViewModel.Auther}ImagePost{Guid.NewGuid()}{extension}";
                        var buf = Convert.FromBase64String(postViewModel.PostImg!);
                        await System.IO.File.WriteAllBytesAsync(env.ContentRootPath + "/wwwroot/post/" + nameImgPost, buf);
                    }
                    var posts = new Post
                    {
                        Title = postViewModel.Title,
                        PostContent = postViewModel.PostContent,
                        Auther = postViewModel.Auther,
                        PostImg = nameImgPost,
                        LinkVideo = postViewModel.LinkVideo,
                        PostDate = DateTime.Now,
                        PostViews = 0,
                        Category = postViewModel.CategoryId,
                        IsPublish = postViewModel.IsPublish
                    };
                    await _context.Posts.AddAsync(posts);
                    await _context.SaveChangesAsync();
                    return Ok(new { success = true, message = "CreatePost" });
                }
                catch (Exception)
                {
                    return Ok(new { success = false, message = StatusCode(StatusCodes.Status500InternalServerError,
                        "Error retrieving data from the database")
                    });
                }
            }

            return Ok(new { success = false, message = "Error CreatePost" });
        }


        [HttpPost("GetEditPost")]
        public async Task<ActionResult> GetEditPost(PostUtilities postUtilities)
        {
            var post = await _context.Posts.FindAsync(postUtilities.PostId);
            if (post == null)
            {
                return Ok(new { success = false, message = "postId null" });
            }


            var Getposts = (from p in _context.Posts
                            where (p.PostId == post.PostId)
                            select new
                            {
                                PostId = p.PostId,
                                Title = p.Title,
                                Auther = p.AboutUsers!.AboutUserId,
                                PostContent = p.PostContent,
                                PostImg = p.PostImg ,
                                LinkVideo = p.LinkVideo,
                                IsPublish = p.IsPublish,
                                CategoryId = p.Categories!.CategoryId,

                            }).FirstOrDefaultAsync();

            return Ok(new { success = true, posts = await Getposts , message = "GetEditPost" });


        }


        [HttpPost("EditPost")]
        public async Task<ActionResult<InfoProfile>> EditPost(PostViewModel postViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var post = await _context.Posts.FindAsync(postViewModel.PostId);
                    if (post == null)
                    {
                        return Ok(new { success = false, message = "postId null" });
                    }

                    string nameImgPost = string.Empty;
                    if (postViewModel.PostImg != null)
                    {
                        string extension = Path.GetExtension(postViewModel!.PostImgName!);
                        nameImgPost = $"{postViewModel.Auther}ImagePost{Guid.NewGuid()}{extension}";
                        var buf = Convert.FromBase64String(postViewModel.PostImg!);
                        await System.IO.File.WriteAllBytesAsync(env.ContentRootPath + "/wwwroot/post/" + nameImgPost, buf);
                        if (postViewModel.PostImgNameOld! != null)
                        {
                            System.IO.File.Delete(env.ContentRootPath + "/wwwroot/post/" + postViewModel.PostImgNameOld!);
                        }
                    }


                    if (post != null)
                    {
                        post.Title = postViewModel.Title;
                        post.PostContent = postViewModel.PostContent;
                        if (postViewModel.PostImg != null) {
                            post.PostImg = nameImgPost;
                        }
                        post.LinkVideo = postViewModel.LinkVideo;
                        post.IsPublish = postViewModel.IsPublish;
                        post.Category = postViewModel.CategoryId;

                        _context.Posts.Attach(post);
                        _context.Entry(post).Property(x => x.Title).IsModified = true;
                        _context.Entry(post).Property(x => x.PostContent).IsModified = true;
                        if (postViewModel.PostImg != null) {
                            _context.Entry(post).Property(x => x.PostImg).IsModified = true;
                        }
                        _context.Entry(post).Property(x => x.LinkVideo).IsModified = true;
                        _context.Entry(post).Property(x => x.IsPublish).IsModified = true;
                        _context.Entry(post).Property(x => x.Category).IsModified = true;

                        await _context.SaveChangesAsync();
                    }
                    return Ok(new { success = true, message = "Succeeded Update" });
                }
                catch (Exception ex)
                {
                    return Ok(new { success = false, message = ex.Message.ToString() });
                }
            }
            return Ok(new { success = false, message = "ModelState IsValid" });

        }






        [HttpPost("DeletePost")]
        public async Task<ActionResult> DeletePost(PostUtilities postUtilities)
        {
            
            try
            {
                string PostImg;
                var post = await _context.Posts.FindAsync(postUtilities.PostId);
                PostImg = post.PostImg;
                if (post == null)
                {
                    return Ok(new { success = false, message = "postId null" });
                }

                var result = _context.Posts.Remove(post!);
                if (result != null)
                {
                    System.IO.File.Delete(env.ContentRootPath + "/wwwroot/post/" + PostImg!);
                    await _context.SaveChangesAsync();
                    return Ok(new { success = true, message = "DeletePost" });
                }
                else
                {
                    return Ok(new { success = false, message = "DeletePost Error" });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, message = "The post cannot be deleted because it contains comments and likes." });
            }
            
        }

    }
}

