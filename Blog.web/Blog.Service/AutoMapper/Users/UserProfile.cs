using AutoMapper;
using Blog.Entity.DTOs.Users;
using Blog.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Service.AutoMapper.Users
{
    public class UserProfile :Profile
    {
        public UserProfile()
        {
            CreateMap<AppUser,UserDTO>().ReverseMap();
            CreateMap<AppUser,UserAddDTO>().ReverseMap();
            CreateMap<AppUser,UserUpdateDTO>().ReverseMap();
            CreateMap<AppUser,UserProfileDTO>().ReverseMap();
        }
    }
}
