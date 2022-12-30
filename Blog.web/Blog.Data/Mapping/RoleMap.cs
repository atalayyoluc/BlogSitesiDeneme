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
    public class RoleMap : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            builder.HasKey(r => r.Id);

            // Index for "normalized" role name to allow efficient lookups
            builder.HasIndex(r => r.NormalizedName).HasName("RoleNameIndex").IsUnique();

            // Maps to the AspNetRoles table
            builder.ToTable("AspNetRoles");

            // A concurrency token for use with the optimistic concurrency checking
            builder.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

            // Limit the size of columns to use efficient database types
            builder.Property(u => u.Name).HasMaxLength(256);
            builder.Property(u => u.NormalizedName).HasMaxLength(256);

            // The relationships between Role and other entity types
            // Note that these relationships are configured with no navigation properties

            // Each Role can have many entries in the UserRole join table
            builder.HasMany<AppUserRole>().WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();

            // Each Role can have many associated RoleClaims
            builder.HasMany<AppRoleClaim>().WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();

            builder.HasData(new AppRole
            { 
             Id=Guid.Parse("AD7DC898-FF9C-4AFB-A4B8-1980A23F15B0"),
             Name="SuperAdmin",
             NormalizedName="SUPERADMIN",
             ConcurrencyStamp=Guid.NewGuid().ToString(),
             
            },
            new AppRole 
            { 
            Id=Guid.Parse("DA34AD8D-32B6-470B-BD6C-D443F31A7D71"),
            Name="Admin",
            NormalizedName="ADMIN",
            ConcurrencyStamp=Guid.NewGuid().ToString(),
            },
            new AppRole 
            {
             Id=Guid.Parse("6FF67B28-BC75-404F-B578-6CE82AD558E5"),
             Name="User",
             NormalizedName="USER",
             ConcurrencyStamp=Guid.NewGuid().ToString(), 
            }
            
            );
        }
    }
}
