using Blog.Core.Entities;
using Blog.Entity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.Mapping
{
    public class UserMap : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasKey(u => u.Id);

            // Indexes for "normalized" username and email, to allow efficient lookups
            builder.HasIndex(u => u.NormalizedUserName).HasName("UserNameIndex").IsUnique();
            builder.HasIndex(u => u.NormalizedEmail).HasName("EmailIndex");

            // Maps to the AspNetUsers table
            builder.ToTable("AspNetUsers");

            // A concurrency token for use with the optimistic concurrency checking
            builder.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

            // Limit the size of columns to use efficient database types
            builder.Property(u => u.UserName).HasMaxLength(256);
            builder.Property(u => u.NormalizedUserName).HasMaxLength(256);
            builder.Property(u => u.Email).HasMaxLength(256);
            builder.Property(u => u.NormalizedEmail).HasMaxLength(256);

            // The relationships between User and other entity types
            // Note that these relationships are configured with no navigation properties

            // Each User can have many UserClaims
            builder.HasMany<AppUserClaim>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
                            
            // Each User canApphave many UserLogins
            builder.HasMany<AppUserLogin>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();
                            
            // Each User canApphave many UserTokens
            builder.HasMany<AppUserToken>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();
                            
            // Each User canApphave many entries in the UserRole join table
            builder.HasMany<AppUserRole>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();

            var superAdmin=new AppUser
            {
                Id=Guid.Parse("14C56317-B033-456D-AB84-82795A0929E4"),
                UserName="superadmin@gmail.com",
                NormalizedUserName="SUPERADMIN@GMAIL.COM",
                Email="superadmin@gmail.com",
                NormalizedEmail="SUPERADMIN@GMAIL.COM",
                PhoneNumber="+905522203214",
                FirstName="Atalay",
                LastName="Yolüç",
                PhoneNumberConfirmed=true,
                EmailConfirmed=true,
                SecurityStamp=new Guid().ToString(),
                ImageId= Guid.Parse("CD2BAD66-9968-436F-ADE6-AACFD337F644"),
            };
            superAdmin.PasswordHash = CreatePasswordHash(superAdmin, "123456");
            var admin = new AppUser
            {
                Id = Guid.Parse("96EC4636-4146-4724-9D3F-849BF5F67E49"),
                UserName = "admin@gmail.com",
                NormalizedUserName = "ADMIN@GMAIL.COM",
                Email = "admin@gmail.com",
                NormalizedEmail = "DMIN@GMAIL.COM",
                PhoneNumber = "+905522209999",
                FirstName = "Admin",
                LastName = "User",
                PhoneNumberConfirmed = false,
                EmailConfirmed = false,
                SecurityStamp = new Guid().ToString(),
                ImageId= Guid.Parse("AB5F1F2C-9C7E-4E62-B597-3186B8BFC705"),
            };
            admin.PasswordHash = CreatePasswordHash(admin, "123456");
            builder.HasData(superAdmin, admin);
        }
        private string CreatePasswordHash(AppUser user , string password)
        {
            var passwordHasher = new PasswordHasher<AppUser>();
            return passwordHasher.HashPassword(user,password);
        } 

    }
}
