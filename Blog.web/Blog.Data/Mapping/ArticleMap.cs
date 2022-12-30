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
    public class ArticleMap : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.HasData(new Article
            {
                Id = Guid.NewGuid(),
                Title = "Asp.net Deneme Başlığı",
                Content = "Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of \"de Finibus Bonorum et Malorum (The Extremes of Good and Evil) by Cicero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum, Lorem ipsum dolor sit amet.., comes from a line in section 1.10.32.The standard chunk of Lorem Ipsum used since the 1500s is reproduced below for those interested. Sections 1.10.32 and 1.10.33 from de Finibus Bonorum et Malorum by Cicero are also reproduced in their exact original form, accompanied by English versions from the 1914 translation by H. Rackham.",
                ViewCount = 15,
                CategoryId = Guid.Parse("6F1FE5F1-789E-40D0-95D4-822E3703DFF1"),
                ImageId = Guid.Parse("AB5F1F2C-9C7E-4E62-B597-3186B8BFC705"),

                CreatedBy = "adminTest",
                CreatedDate = DateTime.Now,
                IsDeleted = false,
                UserId= Guid.Parse("14C56317-B033-456D-AB84-82795A0929E4")



            },
            new Article
            {
                Id = Guid.NewGuid(),
                Title = "ÖrnekDeneme Deneme Başlığı",
                Content = "Deneme Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of de Finibus Bonorum et Malorum (The Extremes of Good and Evil) by Cicero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum, Lorem ipsum dolor sit amet.., comes from a line in section 1.10.32.The standard chunk of Lorem Ipsum used since the 1500s is reproduced below for those interested. Sections 1.10.32 and 1.10.33 from de Finibus Bonorum et Malorum by Cicero are also reproduced in their exact original form, accompanied by English versions from the 1914 translation by H. Rackham.",
                ViewCount = 13,
                CategoryId = Guid.Parse("5F16F260-65BE-4592-B7BC-53771CD0F95E"),
                ImageId= Guid.Parse("CD2BAD66-9968-436F-ADE6-AACFD337F644"),
                CreatedBy = "adminTest",
                CreatedDate = DateTime.Now,
                IsDeleted = false,
                UserId = Guid.Parse("96EC4636-4146-4724-9D3F-849BF5F67E49")


            }

            ) ;
        }
    }
}
