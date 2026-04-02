using System;
using CMSBlazor.Data;
using CMSBlazor.Shared.ViewModels.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMSBlazor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class HomeController : ControllerBase
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
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


        [HttpPost("MostPopularPost")]
        public async Task<ActionResult> MostPopularPost(PostUtilities postUtilities)
        {

            var postscount = _context.Posts.Count().ToString();
            int num = postUtilities.NumGetPost;

            var Getposts = (from p in _context.Posts.OrderByDescending(p => p.PostViews)
                            .Include(p => p.AboutUsers!.ApplicationUsers)
                            where (p.IsPublish == true)
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


        [HttpPost("ListPostByCategory")]
        public async Task<ActionResult> ListPostByCategory(PostUtilities postUtilities)
        {

            var postscount = _context.Posts.Count().ToString();
            int num = postUtilities.NumGetPost;

            var Getposts = (from p in _context.Posts.OrderByDescending(p => p.PostDate)
                            .Include(p => p.AboutUsers!.ApplicationUsers)
                            where (p.IsPublish == true && p.Categories!.CategoryId == postUtilities.CategoryId)
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


        // [Authorize]
        [HttpPost("SinglePost")]
        public async Task<ActionResult> SinglePost(PostUtilities postUtilities)
        {

            var posts = await _context.Posts.FindAsync(postUtilities.PostId);
            if (posts != null)
            {
                posts.PostViews = posts.PostViews + 1;
                _context.Posts.Attach(posts);
                _context.Entry(posts).Property(x => x.PostViews).IsModified = true;
                await _context.SaveChangesAsync();
            }


            var getPost = (from c in _context.Posts
                           where (c.PostId == postUtilities.PostId)
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


            return Ok(new { success = true, post = await getPost });

        }








    }
}

