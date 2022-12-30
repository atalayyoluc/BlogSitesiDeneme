using AutoMapper;
using Blog.Entity.DTOs.Articles;
using Blog.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Service.AutoMapper.Articles
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<ArticleDTO,Article>().ReverseMap();
            CreateMap<ArticleUpdateDTO, ArticleDTO>().ReverseMap();
            CreateMap<ArticleUpdateDTO,Article>().ReverseMap();
            CreateMap<ArticleAddDTO,ArticleDTO>().ReverseMap();
            CreateMap<ArticleAddDTO,Article>().ReverseMap();
            CreateMap<ArticleListDTO, ArticleDTO>().ReverseMap();

        }
    }
}
