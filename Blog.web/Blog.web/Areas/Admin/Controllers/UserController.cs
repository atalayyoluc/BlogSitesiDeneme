using AutoMapper;
using Blog.Entity.DTOs.Articles;
using Blog.Entity.DTOs.Users;
using Blog.Entity.Entities;
using Blog.Service.Extensions;
using Blog.Service.Services.Abstractions;
using Blog.Service.Services.Concretes;
using Blog.web.Const;
using Blog.web.ResultMessages;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System.Data;
using static Blog.web.ResultMessages.Message;

namespace Blog.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly IValidator<AppUser> validator;
        private readonly IToastNotification toast;
        private readonly IMapper mapper;

        public UserController(IUserService userService, IValidator<AppUser> validator, IToastNotification toast, IMapper mapper)
        {
            this.userService = userService;
            this.validator = validator;
            this.toast = toast;
            this.mapper = mapper;
        }
        [HttpGet]
        [Authorize(Roles = $"{RoleConst.EnYetkiliYonetici}")]
        public async Task<IActionResult> Index()
        {
            var result = await userService.GetAllUsersWithRoleAsync();

            return View(result);
        }
        [HttpGet]
        [Authorize(Roles = $"{RoleConst.EnYetkiliYonetici}")]
        public async Task<IActionResult> Add()
        {
            var roles = await userService.GetAllRolesAsync();
            return View(new UserAddDTO { Roles = roles });
        }
        [HttpPost]
        [Authorize(Roles = $"{RoleConst.EnYetkiliYonetici}")]
        public async Task<IActionResult> Add(UserAddDTO userAddDto)
        {
            var map = mapper.Map<AppUser>(userAddDto);
            var validation = await validator.ValidateAsync(map);
            var roles = await userService.GetAllRolesAsync();

            if (ModelState.IsValid)
            {
                var result = await userService.CreateUserAsync(userAddDto);
                if (result.Succeeded)
                {
                    toast.AddSuccessToastMessage(Message.User.Add(userAddDto.Email), new ToastrOptions { Title = "İşlem Başarılı" });
                    return RedirectToAction("Index", "User", new { Area = "Admin" });
                }
                else
                {
                    result.AddToIdentityModelState(this.ModelState);
                    validation.AddToModelState(this.ModelState);
                    return View(new UserAddDTO { Roles = roles });

                }
            }
            return View(new UserAddDTO { Roles = roles });
        }
        [HttpGet]
        [Authorize(Roles = $"{RoleConst.EnYetkiliYonetici}")]
        public async Task<IActionResult> Update(Guid userId)
        {
            var user = await userService.GetAppUserByIdAsync(userId);

            var roles = await userService.GetAllRolesAsync();

            var map = mapper.Map<UserUpdateDTO>(user);
            map.Roles = roles;
            return View(map);
        }
        [HttpPost]
        [Authorize(Roles = $"{RoleConst.EnYetkiliYonetici}")]
        public async Task<IActionResult> Update(UserUpdateDTO userUpdateDto)
        {
            var user = await userService.GetAppUserByIdAsync(userUpdateDto.Id);

            if (user != null)
            {
                var roles = await userService.GetAllRolesAsync();
                if (ModelState.IsValid)
                {
                    var map = mapper.Map(userUpdateDto, user);
                    var validation = await validator.ValidateAsync(map);

                    if (validation.IsValid)
                    {
                        user.UserName = userUpdateDto.Email;
                        user.SecurityStamp = Guid.NewGuid().ToString();
                        var result = await userService.UpdateUserAsync(userUpdateDto);
                        if (result.Succeeded)
                        {
                            toast.AddSuccessToastMessage(Message.User.Update(userUpdateDto.Email), new ToastrOptions { Title = "İşlem Başarılı" });
                            return RedirectToAction("Index", "User", new { Area = "Admin" });
                        }
                        else
                        {
                            result.AddToIdentityModelState(this.ModelState);
                            return View(new UserUpdateDTO { Roles = roles });
                        }
                    }
                    else
                    {
                        validation.AddToModelState(this.ModelState);
                        return View(new UserUpdateDTO { Roles = roles });
                    }
                }
            }
            return NotFound();
        }

        [Authorize(Roles = $"{RoleConst.EnYetkiliYonetici}")]
        public async Task<IActionResult> Delete(Guid userId)
        {
            var result = await userService.DeleteUserAsync(userId);

            if (result.identityResult.Succeeded)
            {
                toast.AddSuccessToastMessage(Message.User.Delete(result.email), new ToastrOptions { Title = "İşlem Başarılı" });
                return RedirectToAction("Index", "User", new { Area = "Admin" });
            }
            else
            {
                result.identityResult.AddToIdentityModelState(this.ModelState);
            }
            return NotFound();
        }
        [HttpGet]
        [Authorize(Roles = $"{RoleConst.EnYetkiliYonetici},{RoleConst.AzYetkiliYonetici},{RoleConst.YetkisizKullanici}")]
        public async Task<IActionResult> Profile()
        {
            var profile = await userService.GetUserProfileAsync();

            return View(profile);
        }
        [HttpPost]
        [Authorize(Roles = $"{RoleConst.EnYetkiliYonetici},{RoleConst.AzYetkiliYonetici},{RoleConst.YetkisizKullanici}")]
        public async Task<IActionResult> Profile(UserProfileDTO userProfileDto)
        {

            if (ModelState.IsValid)
            {
                var result = await userService.UserProfileUpdateAsync(userProfileDto);
                if (result)
                {
                    toast.AddSuccessToastMessage("Profil güncelleme işlemi tamamlandı", new ToastrOptions { Title = "İşlem Başarılı" });
                    return RedirectToAction("Index", "Home", new { Area = "Admin" });
                }
                else
                {
                    var profile = await userService.GetUserProfileAsync();
                    toast.AddErrorToastMessage("Profil güncelleme işlemi tamamlanamadı", new ToastrOptions { Title = "İşlem Başarısız" });
                    return View(profile);
                }
            }
            else
                return NotFound();
        }
    }
}

