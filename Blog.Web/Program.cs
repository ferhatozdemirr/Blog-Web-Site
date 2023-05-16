using Blog.Data.Context;
using Blog.Data.Extensions;
using Blog.Entity.Entities;
using Blog.Service.Describers;
using Blog.Service.Extensions;
using Blog.Web.Filters.ArticleVisitors;
using Microsoft.AspNetCore.Identity;
using NToastNotify;

var builder = WebApplication.CreateBuilder(args);

builder.Services.LoadDataLayerExtensions(builder.Configuration);
builder.Services.LoadServiceLayerExtensions();




builder.Services.AddSession();
builder.Services.AddControllersWithViews(opt =>
{
    opt.Filters.Add<ArticleVisitorFilter>();
})
    
    .AddNToastNotifyNoty(new NotyOptions
{
    ProgressBar = true,
    Timeout = 3000,
})
    .AddRazorRuntimeCompilation();



builder.Services.AddIdentity<AppUser, AppRole>(opt => {

    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireUppercase = false;

})
    .AddRoleManager<RoleManager<AppRole>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddErrorDescriber<CustomIdentityErrorDescriber>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(config =>
{
    config.LoginPath = new PathString("/Admin/Auth/login");
    config.LogoutPath = new PathString("/Admin/Auth/Logout");
    config.Cookie = new CookieBuilder
    {
        Name = "Blog",
        HttpOnly= true,
        SameSite=SameSiteMode.Strict,
        SecurePolicy=CookieSecurePolicy.SameAsRequest //always

    };
    config.SlidingExpiration = true;
    config.ExpireTimeSpan= TimeSpan.FromDays(7);
    config.AccessDeniedPath = new PathString("/Admin/Auth/AccessDenied");
});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseNToastNotify();
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapAreaControllerRoute(
     name:"Admin",
     areaName:"Admin",
     pattern:"Admin/{controller=Home}/{action=Index}/{id?}"
       
        );
    endpoints.MapDefaultControllerRoute();
});

app.Run();
