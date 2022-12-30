using Blog.Entity.DTOs.Users;
using Blog.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;

        public AuthController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        public async Task<IActionResult> Login(UserLoginDTO userLoginDTO)
        {
            if(ModelState.IsValid)
            {
                var user= await userManager.FindByEmailAsync(userLoginDTO.Email);
                if (user != null) 
                {
                    var result = await signInManager.PasswordSignInAsync(user, userLoginDTO.Password, userLoginDTO.RememberMe, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home", new {Area="Admin"});
                    }
                    else
                    {
                        ModelState.AddModelError("", "E-posta yada Şifre Yanlış Lütfen Tekrar Deneyin");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "E-posta yada Şifre Yanlış Lütfen Tekrar Deneyin");
                    return View();
                }
            }
            else
            { 
              return View();
            }
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { Area = "" });
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> AccessDenied()
        {
            return View();
        }


    }
}
