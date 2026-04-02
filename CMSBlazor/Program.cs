using System.Text;
using CMSBlazor.Client;
using CMSBlazor.Controllers;
using CMSBlazor.Data;
using CMSBlazor.Security;
using CMSBlazor.Services.Categories;
using CMSBlazor.Shared.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();



//------------------------------------------------------------------------------------------------------------
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//------------------------------------------------------------------------------------------------------------

// ================ConnectionString Database ======================================

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

///--------------UseSqlite
// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseSqlite(connectionString));

////--------------UseSqlServer
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));


////--------------UseMySql UseMySql
// builder.Services.AddDbContext<AppDbContext>(
// options => options.UseMySql((connectionString), ServerVersion.AutoDetect(connectionString)));

//------------------------------------------------------------------------------------------------------------

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Role Password
    options.Password.RequiredLength = 2;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = true;

    // Role Confrim Account
    options.SignIn.RequireConfirmedEmail = true;
    //options.SignIn.RequireConfirmedPhoneNumber = true;
    //TokenProviders
    options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";


    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(24);

}).AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders()
    .AddTokenProvider<CustomEmailConfirmationTokenProvider
    <ApplicationUser>>("CustomEmailConfirmation");


// Set token life span to 5 hours
builder.Services.Configure<DataProtectionTokenProviderOptions>(o =>
    o.TokenLifespan = TimeSpan.FromMinutes(10));
// Changes token lifespan of just the Email Confirmation Token type
builder.Services.Configure<CustomEmailConfirmationTokenProviderOptions>(o =>
        o.TokenLifespan = TimeSpan.FromMinutes(10));

//-------------------------------------JwtBearerDefaults------------------------------------------------------
builder.Services.AddAuthentication(
   x =>
   {
       x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
       x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
   })
    // builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                //ValidateIssuer = true,
                //ValidateAudience = true,
                //ValidIssuer = builder.Configuration["JwtIssuer"],
                //ValidAudience = builder.Configuration["JwtAudience"],
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF32.GetBytes(builder.Configuration["JwtSecurityKey"]!))
            };
        });


//-------------------------------------Social AddAuthentication------------------------------------------------------


builder.Services.AddAuthentication().AddCookie()
        .AddFacebook(fb => {
            fb.AppId = builder.Configuration
            .GetSection("FacebookSettings").GetValue<string>("AppID")!;

            fb.AppSecret = builder.Configuration
            .GetSection("FacebookSettings").GetValue<string>("AppSecret")!;
        })
        //.AddTwitter(tt => {
        //    tt.ConsumerKey = builder.Configuration
        //    .GetSection("TwitterSettings").GetValue<string>("ApiKey");

        //    tt.ConsumerSecret = builder.Configuration
        //    .GetSection("TwitterSettings").GetValue<string>("ApiKeySecret");

        //    tt.RetrieveUserDetails = true;
        //})
        //.AddMicrosoftAccount(mt => {
        //    mt.ClientId = builder.Configuration
        //    .GetSection("MicrosoftSettings").GetValue<string>("AppId");

        //    mt.ClientSecret = builder.Configuration
        //    .GetSection("MicrosoftSettings").GetValue<string>("SecretId");

        //}).
        .AddGoogle(g => {
            g.ClientId = builder.Configuration
            .GetSection("GoogleSettings").GetValue<string>("ClientId")!;
            g.ClientSecret = builder.Configuration
            .GetSection("GoogleSettings").GetValue<string>("ClientSecret")!;
            g.SaveTokens = true;
        });

//------------------------------------------------------------------------------------------------------------


//================================Manage Claim Role
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CreateRolePolicy", policy => policy.RequireAssertion(context =>
            context.User.IsInRole("SuperAdmin") &&
            context.User.HasClaim(claim => claim.Type == "Create Role" && claim.Value == "true") ||
            context.User.IsInRole("Admin") &&
            context.User.HasClaim(claim => claim.Type == "Create Role" && claim.Value == "true") ||
            context.User.IsInRole("User") &&
            context.User.HasClaim(claim => claim.Type == "Create Role" && claim.Value == "true")

        ));

    options.AddPolicy("DeleteRolePolicy", policy => policy.RequireAssertion(context =>
            context.User.IsInRole("SuperAdmin") &&
            context.User.HasClaim(claim => claim.Type == "Delete Role" && claim.Value == "true") ||
            context.User.IsInRole("Admin") &&
            context.User.HasClaim(claim => claim.Type == "Delete Role" && claim.Value == "true") ||
            context.User.IsInRole("User") &&
            context.User.HasClaim(claim => claim.Type == "Delete Role" && claim.Value == "true")

        ));


    options.AddPolicy("EditRolePolicy", policy => policy.RequireAssertion(context =>
            context.User.IsInRole("SuperAdmin") &&
            context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true") ||
            context.User.IsInRole("Admin") &&
            context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true") ||
            context.User.IsInRole("User") &&
            context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true")


            ));


    //services.AddAuthorization(options =>
    //{
    //    options.AddPolicy("AdminRolePolicy",
    //        policy => policy.RequireRole("Admin"));
    //});


    //options.AddPolicy("SuperAdminPolicy", policy =>
    //                  policy.RequireRole("Admin"));
    //policy.RequireRole("Admin", "User", "Manager"));



});

//========= Add Services=====================================
builder.Services.AddScoped<ICategoriesRepository, CategoriesRepository>();

builder.Services.AddSignalR();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
       new[] { "application/octet-stream" });
});


//--------------------------------CookieEvents----------------------------------------------------------------------------
// builder.Services.AddScoped<CookieEvents>();
// builder.Services.ConfigureApplicationCookie(opt =>
// {
//     opt.EventsType = typeof(CookieEvents);
// });
//------------------------------------------------------------------------------------------------------------



var app = builder.Build();

//-------------


//--------------------------------

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.MapOpenApi();
}


app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
//------------------------------------------------------------------------------------------------------------
app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseResponseCompression();
app.UseSwaggerUI();
app.UseSwagger();
app.UseAuthentication();
app.MapRazorPages();
//--------------------------------
app.MapHub<HubController>("/hubcontroller");
//--------------------------------


//------------------------------------------------------------------------------------------------------------


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


app.UseAntiforgery();
// app.MapStaticAssets();
app.UseStaticFiles();

// app.MapRazorComponents<App>()
//     .AddInteractiveServerRenderMode()
//     .AddInteractiveWebAssemblyRenderMode()
//     .AddAdditionalAssemblies(typeof(CMSBlazor.Client._Imports).Assembly);

app.Run();