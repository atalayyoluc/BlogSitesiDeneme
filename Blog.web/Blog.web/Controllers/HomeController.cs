using Blog.Service.Services.Abstractions;
using Blog.Service.Services.Concretes;
using Blog.web.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Blog.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IArticleService articleService;
        private readonly ICategoryService categoryService;

        public HomeController(ILogger<HomeController> logger,IArticleService articleService,ICategoryService categoryService)
        {
            _logger = logger;
            this.articleService=articleService;
            this.categoryService = categoryService;
        }

        public async Task<IActionResult> Index(Guid?categoryId,int cuurentPage=1,int pageSize=3,bool isAcsending=false)
        {
            var articles = await articleService.GetAllByPagingAsync(categoryId,cuurentPage,pageSize,isAcsending); 
            return View(articles);
        }

        [HttpGet]
        public async Task<IActionResult> GetByArticle(Guid articleId)
        {
            var article = await articleService.GetArticleWithCategoryNonDeletedAsync(articleId);
            return View(article);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        public async Task<IActionResult> Search(string keyword, int cuurentPage = 1, int pageSize = 3, bool isAcsending = false)
        {
            var articles = await articleService.Search(keyword, cuurentPage, pageSize, isAcsending);
            ViewBag.keyword = keyword;
            return View(articles);
        }
   
    }
}