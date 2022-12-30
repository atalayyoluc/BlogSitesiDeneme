using Blog.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.Mapping
{
    public class CategoryMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(new Category
            {
                Id = Guid.Parse("6F1FE5F1-789E-40D0-95D4-822E3703DFF1"),
                Name = "Asp.Net Core",
                CreatedBy = "adminTest",
                CreatedDate = DateTime.Now,
                IsDeleted = false,
            },
            new Category
            {
                Id = Guid.Parse("5F16F260-65BE-4592-B7BC-53771CD0F95E"),
                Name = "Asp.Net Core",
                CreatedBy = "adminTest",
                CreatedDate = DateTime.Now,
                IsDeleted = false,

            }
            
            );
           
                
        }
    }
}
