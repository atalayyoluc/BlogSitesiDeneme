using Blog.Entity.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Service.FluentValidations
{
    public class ArticleValidator :AbstractValidator<Article>
    {
        public ArticleValidator()
        {
            RuleFor(x => x.Title).
                NotNull().
                NotEmpty()
                .MinimumLength(10).MaximumLength(45)
                .WithName("Başlık");

            RuleFor(x=>x.Content)
                .NotNull()
                .NotEmpty()
                .MinimumLength(100).MaximumLength(5000)
                .WithName("Makale Metni");
        }
    }
}
