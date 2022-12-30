using Blog.Service.FluentValidations;
using Blog.Service.Helpers.Images;
using Blog.Service.Services.Abstractions;
using Blog.Service.Services.Concretes;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Reflection;
using YoutubeBlog.Service.Services.Concrete;

namespace Blog.Service.Extensions
{
    public static class ServiceLayerExtensions
    {
        public static IServiceCollection LoadServiceLayerExtension(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDashBordService, DashBordService>();
            services.AddScoped<IImageHelper, ImageHelper>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAutoMapper(assembly);


            services.AddControllersWithViews()
             .AddFluentValidation(opt =>
             {
                 opt.RegisterValidatorsFromAssemblyContaining<ArticleValidator>();
                 opt.DisableDataAnnotationsValidation = true;
                 opt.ValidatorOptions.LanguageManager.Culture = new CultureInfo("tr");
             });


            return services;
        }

    }
}
