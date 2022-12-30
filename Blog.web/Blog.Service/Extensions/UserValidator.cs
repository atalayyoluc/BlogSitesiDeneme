using Blog.Entity.Entities;
using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Service.Extensions
{
    public class UserValidator :AbstractValidator<AppUser>
    {
        public UserValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(20)
                .WithName("Ad");
            RuleFor(x => x.LastName)
               .NotEmpty()
               .MinimumLength(3)
               .MaximumLength(20)
               .WithName("Soyad");
            RuleFor(x => x.PhoneNumber)
               .NotEmpty()
               .MinimumLength(11)
               .MaximumLength(13)
               .WithName("Telefon Numarası");
        }

    }
}
