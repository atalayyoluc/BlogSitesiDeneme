using AutoMapper;
using Blog.Entity.DTOs.Articles;
using Blog.Entity.DTOs.Categories;
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
    public class CategoryController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly IValidator<Category> validator;
        private readonly IMapper mapper;
        private readonly IToastNotification toastNotification;

        public CategoryController(ICategoryService categoryService,IValidator<Category> validator,IMapper mapper,IToastNotification toastNotification)
        {
            this.categoryService = categoryService;
            this.validator = validator;
            this.mapper = mapper;
            this.toastNotification = toastNotification;
        }
        [HttpGet]
        [Authorize(Roles = $"{RoleConst.EnYetkiliYonetici},{RoleConst.AzYetkiliYonetici},{RoleConst.YetkisizKullanici}")]
        public async  Task<IActionResult> Index()
        {
            var categories = await categoryService.GetAllCategoriesNonDeleted();
            return View(categories);
        }
        [HttpGet]
        [Authorize(Roles = $"{RoleConst.EnYetkiliYonetici},{RoleConst.AzYetkiliYonetici}")]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = $"{RoleConst.EnYetkiliYonetici},{RoleConst.AzYetkiliYonetici}")]
        public async Task<IActionResult> Add(CategoryAddDTO categoryAddDTO)
        {
            var map = mapper.Map<Category>(categoryAddDTO);
            var result = await validator.ValidateAsync(map);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                return View();
            }
           
                await categoryService.CreateCategoryAsync(categoryAddDTO);
            toastNotification.AddSuccessToastMessage(Message.Category.Add(categoryAddDTO.Name), new ToastrOptions { Title = "Ekleme İşlemi Başarılı" });
            return RedirectToAction("Index", "Category", new {Area="Admin"});
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleConst.EnYetkiliYonetici},{RoleConst.AzYetkiliYonetici}")]
        public async Task<IActionResult> AddWithAjax([FromBody] CategoryAddDTO categoryAddDTO)
        {
            var map = mapper.Map<Category>(categoryAddDTO);
            var result = await validator.ValidateAsync(map);

            if (result.IsValid) { 
            await categoryService.CreateCategoryAsync(categoryAddDTO);
            
            toastNotification.AddSuccessToastMessage(Message.Category.Add(categoryAddDTO.Name), new ToastrOptions { Title = "Ekleme İşlemi Başarılı" });
            
                return Json(Message.Category.Add(categoryAddDTO.Name));
            }
            else
            {
                toastNotification.AddErrorToastMessage(result.Errors.First().ErrorMessage, new ToastrOptions { Title = "İşlem Başarısız" });
                return Json(result.Errors.First().ErrorMessage);    
            }
        }

        [HttpGet]
        [Authorize(Roles = $"{RoleConst.EnYetkiliYonetici},{RoleConst.AzYetkiliYonetici}")]
        public async Task<IActionResult> Update(Guid categoryId)
        {
            var category = await categoryService.GetCategoryByGuid(categoryId);
            var map = mapper.Map<Category, CategoryUpdateDTO>(category);
            return View(map);   
            
         }


        [HttpPost]
        [Authorize(Roles = $"{RoleConst.EnYetkiliYonetici},{RoleConst.AzYetkiliYonetici}")]
        public async Task<IActionResult> Update(CategoryUpdateDTO categoryUpdateDTO)
        {
            var map = mapper.Map<Category>(categoryUpdateDTO);
            var result = await validator.ValidateAsync(map);
            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                return View();
            }
                var name = await categoryService.UpdateCategoryAsync(categoryUpdateDTO);
                toastNotification.AddSuccessToastMessage(Message.Category.Add(categoryUpdateDTO.Name), new ToastrOptions { Title = "Ekleme İşlemi Başarılı" });
                return RedirectToAction("Index", "Category", new { Area = "Admin" });
        }
        [Authorize(Roles = $"{RoleConst.EnYetkiliYonetici},{RoleConst.AzYetkiliYonetici}")]
        public async Task<IActionResult> Delete(Guid categoryId) 
        {
            var name = await categoryService.SafeDeleteCategoryAsync(categoryId);
            toastNotification.AddWarningToastMessage(Message.Category.Delete(name), new ToastrOptions { Title = "Silme İşlemi Başarı" });

            return RedirectToAction("Index", "Category", new { Area = "Admin" });
        }
        [Authorize(Roles = $"{RoleConst.EnYetkiliYonetici}")]
        public async Task<IActionResult> DeletedCategory()
        {
            var categories = await categoryService.GetAllCategoriesDeleted();
            return View(categories);
        }
        [Authorize(Roles = $"{RoleConst.EnYetkiliYonetici}")]
        public async Task<IActionResult> UndoDeleted(Guid categoryId)
        {
            var name = await categoryService.UndoDeleteCategoryAsync(categoryId);
            toastNotification.AddSuccessToastMessage(Message.Category.UndoDelete(name), new ToastrOptions { Title = "Silme İşlemi Başarı" });

            return RedirectToAction("DeletedCategory", "Category", new { Area = "Admin" });
        }
    }
}
