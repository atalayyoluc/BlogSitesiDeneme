using Blog.Entity.DTOs.Categories;
using Blog.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Entity.DTOs.Articles
{
    public class ArticleDTO
    {
        public Guid Id { get; set; }    
        public string Title { get; set; }
        public string Content { get; set; }
        public CategoryDTO Category { get; set; }
        public string CreatedBy { get; set; }  
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Image Image { get; set; }
        public int ViewCount { get; set; } 

    }
}
