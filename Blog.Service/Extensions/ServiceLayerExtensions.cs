using Blog.Service.FluentValidations;
using Blog.Service.Helpers.Images;
using Blog.Service.Services.Abstractions;
using Blog.Service.Services.Concrete;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using YoutubeBlog.Service.Services.Concrete;

namespace Blog.Service.Extensions
{
    public static class ServiceLayerExtensions
    {
        public static IServiceCollection LoadServiceLayerExtensions(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.AddScoped<IArticleService,ArticleService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IImageHelper, ImageHelper>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDashboardService, DashboardService>();




            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); //Login olan kullanıxıyı bulacağız

            services.AddAutoMapper(assembly);

            services.AddControllersWithViews().AddFluentValidation(opt =>
            {
                opt.RegisterValidatorsFromAssemblyContaining<ArticleValidator>();
                opt.DisableDataAnnotationsValidation=true; //claslara [Key] yazmamak adına bunu true ya çekiyoruz
                opt.ValidatorOptions.LanguageManager.Culture = new System.Globalization.CultureInfo("tr"); //dil değişikliği


            });

            return services;
        }
    }
}
