using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CMSBlazor.Shared.Models;

namespace CMSBlazor.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    //public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options)
         : base(options)
        { }

        public DbSet<AboutUser> AboutUsers { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ReComment> ReComments { get; set; }
        public DbSet<LikePost> LikePosts { get; set; }
        public DbSet<LikeComment> LikeComments { get; set; }
        public DbSet<LikeReComment> LikeReComments { get; set; }
        public DbSet<GenerateToken> GenerateTokens { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

        }






    }
}

