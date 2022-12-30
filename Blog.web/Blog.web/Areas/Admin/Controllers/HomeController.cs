using Blog.Entity.Entities;
using Blog.Service.Services.Abstractions;
using Blog.Service.Services.Concretes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Blog.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class HomeController : Controller
    {
        

        private readonly IArticleService articleService;
        private readonly IDashBordService dashBordService;

        public HomeController(IArticleService articleService,IDashBordService dashBoardService)
        {
            this.articleService = articleService;
            this.dashBordService = dashBoardService;
        }
        public async Task<IActionResult> Index()
        {
            var article=await articleService.GetAllArticleWithCategoryNonDeletedAsync();
           
            return View(article);
        }
        [HttpGet]
        public async Task<IActionResult> YearlyArticleCounts()
        {
            var count = await dashBordService.GetYearlyArticleCounts();
            return Json(JsonConvert.SerializeObject(count));
        }
        [HttpGet]
        public async Task<IActionResult> TotalArticleCount()
        {
            var count = await dashBordService.GetTotalArticleCount();
            return Json(count);
        }
        [HttpGet]
        public async Task<IActionResult> TotalCategoryCount()
        {
            var count = await dashBordService.GetTotalArticleCount();
            return Json(count);
        }



    }
}
