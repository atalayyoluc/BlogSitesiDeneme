using AutoMapper;
using Blog.Entity.DTOs.Articles;
using Blog.Entity.Entities;
using Blog.Service.Extensions;
using Blog.Service.Services.Abstractions;
using Blog.web.Const;
using Blog.web.ResultMessages;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace Blog.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ArticleController : Controller

    {
        private readonly IArticleService articleService;
        private readonly ICategoryService categoryService;
        private readonly IMapper mapper;
        private readonly IValidator<Article> validator;
        private readonly IToastNotification toastNotification;

        public ArticleController(IArticleService articleService ,ICategoryService categoryService,IMapper mapper,IValidator<Article> validator,IToastNotification toastNotification)
        {
            this.articleService = articleService;
            this.categoryService = categoryService;
            this.mapper = mapper;
            this.validator = validator;
            this.toastNotification = toastNotification;
        }
        [HttpGet]
        [Authorize(Roles = $"{RoleConst.EnYetkiliYonetici},{RoleConst.AzYetkiliYonetici},{RoleConst.YetkisizKullanici}")]
        public async Task<IActionResult> Index()
        {
            var articles= await articleService.GetAllArticleWithCategoryNonDeletedAsync();
            return View(articles);
        }

        [HttpGet]
        [Authorize(Roles = $"{RoleConst.EnYetkiliYonetici},{RoleConst.AzYetkiliYonetici}")]
        public async Task<IActionResult> Add()
        {
      
            var categories = await categoryService.GetAllCategoriesNonDeleted();
            return View(new ArticleAddDTO { Categories=categories });  
        }
        [HttpPost]
        [Authorize(Roles = $"{RoleConst.EnYetkiliYonetici},{RoleConst.AzYetkiliYonetici}")]
        public async Task<IActionResult>Add(ArticleAddDTO articleAddDTO)
        {
            var map = mapper.Map<Article>(articleAddDTO);
            var result = await validator.ValidateAsync(map);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                var categories = await categoryService.GetAllCategoriesNonDeleted();
                return View(new ArticleAddDTO { Categories = categories });
            }

            await articleService.CreateArticleAsync(articleAddDTO);
            toastNotification.AddSuccessToastMessage(Message.Article.Add(articleAddDTO.Title),new ToastrOptions { Title="Ekleme İşlemi Başarılı"});
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Authorize(Roles = $"{RoleConst.EnYetkiliYonetici},{RoleConst.AzYetkiliYonetici}")]
        public async Task<IActionResult> Update(Guid articleId) 
        {
            var article=await articleService.GetArticleWithCategoryNonDeletedAsync(articleId);
            var categories = await categoryService.GetAllCategoriesNonDeleted();
            var articleUpdateDTO=mapper.Map<ArticleUpdateDTO>(article);
            articleUpdateDTO.Categories=categories; 
            return View(articleUpdateDTO);
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleConst.EnYetkiliYonetici},{RoleConst.AzYetkiliYonetici}")]
        public async Task<IActionResult> Update(ArticleUpdateDTO articleUpdateDTO)
        {
            var map = mapper.Map<Article>(articleUpdateDTO);
            var result= await validator.ValidateAsync(map);
            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                var categories = await categoryService.GetAllCategoriesNonDeleted();
                return View(new ArticleUpdateDTO { Categories = categories });

            }
           var title= await articleService.UpdateArticleAsync(articleUpdateDTO);
            toastNotification.AddSuccessToastMessage(Message.Article.Update(title), new ToastrOptions { Title = "Güncelleme İşlemi Başarılı" });
          
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Authorize(Roles = $"{RoleConst.EnYetkiliYonetici},{RoleConst.AzYetkiliYonetici}")]
        public async Task<IActionResult> Delete(Guid articleId)
        {
         var title=await articleService.SafeDeleteArticleAsync(articleId);
            toastNotification.AddWarningToastMessage(Message.Article.Delete(title), new ToastrOptions { Title = "Silme İşlemi Başarılı" });
            return RedirectToAction("Index");
        }


        [HttpGet]
        [Authorize(Roles = $"{RoleConst.EnYetkiliYonetici}")]
        public async Task<IActionResult> DeletedArticles()
        {
            var articles = await articleService.GetAllArticleDeletedAsync();
            return View(articles);
        }
        [HttpGet]
        [Authorize(Roles = $"{RoleConst.EnYetkiliYonetici}")]
        public async Task<IActionResult> UndoDeleted(Guid articleId)
        {
            var title = await articleService.UndoDeleteArticleAsync(articleId);
            toastNotification.AddSuccessToastMessage(Message.Article.UndoDelete(title), new ToastrOptions { Title = "Geri İşlemi Başarılı" });
            return RedirectToAction("DeletedArticles");
        }




    }
}
