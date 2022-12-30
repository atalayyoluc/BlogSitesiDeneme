using Blog.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Entity.Entities
{
    public class AppUser :IdentityUser<Guid> ,IEntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid ImageId { get; set; } = Guid.Parse("fd16d8c8-3cf8-4830-9f04-e18ba46eac15");
        public Image Image { get; set; }
        public ICollection<Article> Articles { get; set; }
    }
}
