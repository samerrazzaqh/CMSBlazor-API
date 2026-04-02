using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMSBlazor.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Block = table.Column<bool>(type: "bit", nullable: false),
                    BlockTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CatName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AboutUsers",
                columns: table => new
                {
                    AboutUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Profession = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBurth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Skills = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Experience = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UrlImageProfile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UrlImageCover = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutUsers", x => x.AboutUserId);
                    table.ForeignKey(
                        name: "FK_AboutUsers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GenerateTokens",
                columns: table => new
                {
                    GenerateTokenId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TokenFromEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TokenEmailConfirmation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TokenPasswordReset = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenerateTokens", x => x.GenerateTokenId);
                    table.ForeignKey(
                        name: "FK_GenerateTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    PostId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PostContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LinkVideo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PostImg = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Auther = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PostDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PostViews = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    IsPublish = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_Posts_AboutUsers_Auther",
                        column: x => x.Auther,
                        principalTable: "AboutUsers",
                        principalColumn: "AboutUserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Posts_Categories_Category",
                        column: x => x.Category,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    CommentId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TextComment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LCODate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PostId = table.Column<long>(type: "bigint", nullable: false),
                    AboutUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_Comments_AboutUsers_AboutUserId",
                        column: x => x.AboutUserId,
                        principalTable: "AboutUsers",
                        principalColumn: "AboutUserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LikePosts",
                columns: table => new
                {
                    LikePostId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LikPost = table.Column<int>(type: "int", nullable: false),
                    PostId = table.Column<long>(type: "bigint", nullable: false),
                    AboutUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikePosts", x => x.LikePostId);
                    table.ForeignKey(
                        name: "FK_LikePosts_AboutUsers_AboutUserId",
                        column: x => x.AboutUserId,
                        principalTable: "AboutUsers",
                        principalColumn: "AboutUserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LikePosts_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LikeComments",
                columns: table => new
                {
                    LikeCommentId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LikComment = table.Column<int>(type: "int", nullable: false),
                    PostId = table.Column<long>(type: "bigint", nullable: false),
                    AboutUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CommentId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikeComments", x => x.LikeCommentId);
                    table.ForeignKey(
                        name: "FK_LikeComments_AboutUsers_AboutUserId",
                        column: x => x.AboutUserId,
                        principalTable: "AboutUsers",
                        principalColumn: "AboutUserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LikeComments_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "CommentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LikeComments_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReComments",
                columns: table => new
                {
                    ReCommentId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReTextComment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LCODate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PostId = table.Column<long>(type: "bigint", nullable: false),
                    AboutUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CommentId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReComments", x => x.ReCommentId);
                    table.ForeignKey(
                        name: "FK_ReComments_AboutUsers_AboutUserId",
                        column: x => x.AboutUserId,
                        principalTable: "AboutUsers",
                        principalColumn: "AboutUserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReComments_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "CommentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReComments_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LikeReComments",
                columns: table => new
                {
                    LikeReCommentId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LikReComment = table.Column<int>(type: "int", nullable: false),
                    PostId = table.Column<long>(type: "bigint", nullable: false),
                    AboutUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CommentId = table.Column<long>(type: "bigint", nullable: false),
                    ReCommentId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikeReComments", x => x.LikeReCommentId);
                    table.ForeignKey(
                        name: "FK_LikeReComments_AboutUsers_AboutUserId",
                        column: x => x.AboutUserId,
                        principalTable: "AboutUsers",
                        principalColumn: "AboutUserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LikeReComments_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "CommentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LikeReComments_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LikeReComments_ReComments_ReCommentId",
                        column: x => x.ReCommentId,
                        principalTable: "ReComments",
                        principalColumn: "ReCommentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AboutUsers_UserId",
                table: "AboutUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AboutUserId",
                table: "Comments",
                column: "AboutUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId",
                table: "Comments",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_GenerateTokens_UserId",
                table: "GenerateTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LikeComments_AboutUserId",
                table: "LikeComments",
                column: "AboutUserId");

            migrationBuilder.CreateIndex(
                name: "IX_LikeComments_CommentId",
                table: "LikeComments",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_LikeComments_PostId",
                table: "LikeComments",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_LikePosts_AboutUserId",
                table: "LikePosts",
                column: "AboutUserId");

            migrationBuilder.CreateIndex(
                name: "IX_LikePosts_PostId",
                table: "LikePosts",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_LikeReComments_AboutUserId",
                table: "LikeReComments",
                column: "AboutUserId");

            migrationBuilder.CreateIndex(
                name: "IX_LikeReComments_CommentId",
                table: "LikeReComments",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_LikeReComments_PostId",
                table: "LikeReComments",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_LikeReComments_ReCommentId",
                table: "LikeReComments",
                column: "ReCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_Auther",
                table: "Posts",
                column: "Auther");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_Category",
                table: "Posts",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_ReComments_AboutUserId",
                table: "ReComments",
                column: "AboutUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReComments_CommentId",
                table: "ReComments",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_ReComments_PostId",
                table: "ReComments",
                column: "PostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "GenerateTokens");

            migrationBuilder.DropTable(
                name: "LikeComments");

            migrationBuilder.DropTable(
                name: "LikePosts");

            migrationBuilder.DropTable(
                name: "LikeReComments");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "ReComments");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "AboutUsers");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
