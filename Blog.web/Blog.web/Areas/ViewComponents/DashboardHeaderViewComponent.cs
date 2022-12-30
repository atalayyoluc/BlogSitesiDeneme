using AutoMapper;
using Blog.Entity.DTOs.Users;
using Blog.Entity.Entities;
using Blog.Service.Helpers.Images;
using Blog.Service.Services.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.web.Areas.ViewComponents
{
    public class DashboardHeaderViewComponent :ViewComponent
    {
        private readonly IMapper mapper;
        private readonly IUserService userService;

        public DashboardHeaderViewComponent(IUserService userService,IMapper mapper)
        {
       
            this.mapper = mapper;
            this.userService = userService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
         var map= await userService.GetAllUsersWithRoleAndImageAsync();

            return View(map);
        }
    }
}
