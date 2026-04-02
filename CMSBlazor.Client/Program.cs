using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using CMSBlazor.Client;
using CMSBlazor.Client.Helpers;
using CMSBlazor.Client.Models;
using CMSBlazor.Client.Service.Account;
using CMSBlazor.Client.Service.Administration;
using CMSBlazor.Client.Service.Categories;
using CMSBlazor.Client.Service.Comment;
using CMSBlazor.Client.Service.Home;
using CMSBlazor.Client.Service.Posts;
using CMSBlazor.Client.Service.Profile;
using CMSBlazor.Client.Service.ReComment;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddOptions();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(CMSConstant.BaseApiAddress) });




builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
builder.Services.AddAutoMapper(typeof(SettingsProfile));

//-----------------------------------------------------------------
builder.Services.AddHttpClient<IAccountService, AccountService>();
builder.Services.AddHttpClient<IAdministrationService, AdministrationService>();
builder.Services.AddHttpClient<ICategoryService, CategoryService>();
builder.Services.AddHttpClient<IProfileService, ProfileService>();
builder.Services.AddHttpClient<IPostService, PostService>();
builder.Services.AddHttpClient<IHomeService, HomeService>();
builder.Services.AddHttpClient<ICommentService, CommentService>();
builder.Services.AddHttpClient<IReCommentService, ReCommentService>();
//-----------------------------------------------------------------
// builder.Services.AddApiAuthorization();



await builder.Build().RunAsync();