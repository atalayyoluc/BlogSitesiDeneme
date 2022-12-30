using Blog.Entity.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Service.FluentValidations
{
    public class CategoryValidator :AbstractValidator<Category>
    {

        public CategoryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .NotNull()
                .MinimumLength(10)
                .MaximumLength(30)
                .WithName("Kategori Adı");
        }
    }
}
