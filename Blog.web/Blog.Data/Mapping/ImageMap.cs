using Blog.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.Mapping
{
    public class ImageMap : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.HasData(new Image
            {
                Id = Guid.Parse("AB5F1F2C-9C7E-4E62-B597-3186B8BFC705"),
                FileName = "images/testimage",
                FileType = "jfif",
                CreatedBy = "adminTest",
                CreatedDate = DateTime.Now,
                IsDeleted = false,
            },
            new Image 
            {
                Id = Guid.Parse("CD2BAD66-9968-436F-ADE6-AACFD337F644"),
                FileName = "images/testimage2",
                FileType = "jfif",
                CreatedBy = "adminTest",
                CreatedDate = DateTime.Now,
                IsDeleted = false,
            }


            );
        }
    }
}
