using AutoMapper;
using Blog.Data.UnitOfWorks;
using Blog.Entity.DTOs.Articles;
using Blog.Entity.Entities;
using Blog.Entity.Enums;
using Blog.Service.Extensions;
using Blog.Service.Helpers.Images;
using Blog.Service.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace Blog.Service.Services.Concretes
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IImageHelper imageHelper;
        private readonly ClaimsPrincipal user;

        public ArticleService(IUnitOfWork unitOfWork, IMapper mapper,IHttpContextAccessor httpContextAccessor,IImageHelper imageHelper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            this.imageHelper = imageHelper;
            user = httpContextAccessor.HttpContext.User;
        }

        public async Task CreateArticleAsync(ArticleAddDTO articleAddDTO)
        {
            var userId = user.GetLoggedInUserId();
            var userEmail = user.GetLoggedInUserEmail();

            var imageUpload = await imageHelper.Upload(articleAddDTO.Title,articleAddDTO.Photo,ImageType.Post);
            Image image = new(imageUpload.FullName,articleAddDTO.Photo.ContentType,userEmail);
           await unitOfWork.GetRepository<Image>().AddAsync(image);


            var article = new Article(articleAddDTO.Title,articleAddDTO.Content,articleAddDTO.CategoryId,image.Id,userId,userEmail);
    
            await unitOfWork.GetRepository<Article>().AddAsync(article);
            await unitOfWork.SaveAsync();

        }

        public async Task<List<ArticleDTO>> GetAllArticleWithCategoryNonDeletedAsync()
        {
          var articles= await unitOfWork.GetRepository<Article>().GetAllAsync(x=>!x.IsDeleted, x=>x.Category,x=>x.Image);
            var map = mapper.Map<List<ArticleDTO>>(articles);
            return map;
        }

        public async Task<ArticleDTO> GetArticleWithCategoryNonDeletedAsync(Guid articleId)
        {
            var article = await unitOfWork.GetRepository<Article>().GetAsync(x => !x.IsDeleted && x.Id==articleId, x => x.Category, x => x.Image);
            article.ViewCount = 1;
            var map = mapper.Map<ArticleDTO>(article);
            return map;
        }
        
       public async Task<string> UpdateArticleAsync(ArticleUpdateDTO articleUpdateDTO)
        {
            var userEmail = user.GetLoggedInUserEmail();
            var article = await unitOfWork.GetRepository<Article>().GetAsync(x => !x.IsDeleted && x.Id == articleUpdateDTO.Id, x => x.Category,x=>x.Image);

            if (articleUpdateDTO.Photo != null)
            {
                imageHelper.Delete(article.Image.FileName);
                var imageUpload = await imageHelper.Upload(articleUpdateDTO.Title,articleUpdateDTO.Photo,ImageType.Post);
                Image image = new(imageUpload.FullName,articleUpdateDTO.Photo.ContentType,userEmail);
            await unitOfWork.GetRepository<Image>().AddAsync(image);

                article.ImageId= image.Id;
            }

            article.Content= articleUpdateDTO.Content;
            article.Title= articleUpdateDTO.Title;
            article.CategoryId= articleUpdateDTO.CategoryId;
            article.ModifiedDate = DateTime.Now;
            article.ModifiedBy =userEmail;

            await unitOfWork.GetRepository<Article>().UpdateAsync(article);
            await unitOfWork.SaveAsync();

            return article.Title;
        }
        public async Task<string> SafeDeleteArticleAsync(Guid articleId)
        {
            var userEmail = user.GetLoggedInUserEmail();
            var article=await unitOfWork.GetRepository<Article>().GetByGuidAsync(articleId);
            article.IsDeleted = true;
            article.DeletedDate= DateTime.Now;
            article.DeletedBy = userEmail;
            await unitOfWork.GetRepository<Article>().UpdateAsync(article);
            await unitOfWork.SaveAsync();

            return article.Title;
        }

        public async Task<List<ArticleDTO>> GetAllArticleDeletedAsync()
        {
            var articles = await unitOfWork.GetRepository<Article>().GetAllAsync(x=>x.IsDeleted,x=>x.Category);
            var map = mapper.Map<List<ArticleDTO>>(articles);
            return map;
        }

        public async Task<string> UndoDeleteArticleAsync(Guid articleId)
        {
            var userEmail = user.GetLoggedInUserEmail();
            var article = await unitOfWork.GetRepository<Article>().GetByGuidAsync(articleId);
            article.IsDeleted = false;
            article.DeletedDate =null;
            article.DeletedBy = null;
            await unitOfWork.GetRepository<Article>().UpdateAsync(article);
            await unitOfWork.SaveAsync();

            return article.Title;
        }
        public async Task<ArticleListDTO> GetAllByPagingAsync(Guid? categoryId,int currentPage=1,int pageSize=3, bool isAscending=false)
        {
            pageSize = pageSize > 20 ? 20 : pageSize;

            var articles = categoryId == null ? await unitOfWork.GetRepository<Article>().GetAllAsync(a => !a.IsDeleted, a => a.Category, a => a.Image)
                : await unitOfWork.GetRepository<Article>().GetAllAsync(a=>a.CategoryId==categoryId&& !a.IsDeleted,a=>a.Category,a=>a.Image);

            var sortedArticles = isAscending ? articles.OrderBy(a => a.CreatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList() :
                    articles.OrderByDescending(a => a.CreatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
          

            return new ArticleListDTO
            {
                Articles = sortedArticles ,
                CategoryId = categoryId == null ? null : categoryId.Value,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = articles.Count,
                IsAscending = isAscending
            };
        }   


    }

}
