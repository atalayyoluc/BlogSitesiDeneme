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
    public class UserRoleMap : IEntityTypeConfiguration<AppUserRole>
    {
        public void Configure(EntityTypeBuilder<AppUserRole> builder)
        {
            builder.HasKey(r => new { r.UserId, r.RoleId });

            // Maps to the AspNetUserRoles table
            builder.ToTable("AspNetUserRoles");

            builder.HasData(new AppUserRole
            {
                UserId= Guid.Parse("14C56317-B033-456D-AB84-82795A0929E4"),
                RoleId= Guid.Parse("AD7DC898-FF9C-4AFB-A4B8-1980A23F15B0")
            },
            new AppUserRole
            {
                UserId= Guid.Parse("96EC4636-4146-4724-9D3F-849BF5F67E49"),
                RoleId = Guid.Parse("DA34AD8D-32B6-470B-BD6C-D443F31A7D71")
            });
        }
    }
}
