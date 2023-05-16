using Blog.Entity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static System.Net.Mime.MediaTypeNames;

namespace Blog.Data.Mapping
{
    public class UserMap : IEntityTypeConfiguration<AppUser>
    {
        //IdentityASP.NET Core model özelleştirme
        //https://learn.microsoft.com/tr-tr/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-6.0


        public void Configure(EntityTypeBuilder<AppUser> builder)
        {

            // Primary key
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

            // Each User can have many UserLogins
            builder.HasMany<AppUserLogin>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();

            // Each User can have many UserTokens
            builder.HasMany<AppUserToken>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();

            // Each User can have many entries in the UserRole join table
            builder.HasMany<AppUserRole>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();

            var superadmin = new AppUser
                {
                Id = Guid.Parse("45CB1797-8868-4C0D-846D-D7E5C3E4BB26"),
                UserName = "superadmin@gmail.com",
                NormalizedUserName = "SUPERADMIN@GMAIL.COM",
                Email = "superadmin@gmail.com",
                NormalizedEmail = "SUPERADMIN@GMAIL.COM",
                PhoneNumber = "+05519999999",
                FirstName = "Ferhat",
                LastName = "Ozdemir",
                PhoneNumberConfirmed = true,
                EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString(),
                ImageId= Guid.Parse("D491F90B-90AC-468A-A076-0ED56AC15E6E"),

            };

            superadmin.PasswordHash = CreatePasswordHash(superadmin, "123456");

            var admin = new AppUser
            {
                Id = Guid.Parse("66DC5149-6B91-462C-8EBC-2CD2C18C7D35"),
                UserName = "admin@gmail.com",
                NormalizedUserName = "ADMIN@GMAIL.COM",
                Email = "admin@gmail.com",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                PhoneNumber = "+05519999988",
                FirstName = "Admin",
                LastName = "User",
                PhoneNumberConfirmed = false,
                EmailConfirmed = false,
                SecurityStamp = Guid.NewGuid().ToString(),
                ImageId = Guid.Parse("D491F90B-90AC-468A-A076-0ED56AC15E6E"),

            };
            superadmin.PasswordHash = CreatePasswordHash(admin, "123456");
            builder.HasData(superadmin, admin);


        }
        private string CreatePasswordHash(AppUser user, string password)
        {
            var passwordhaser = new PasswordHasher<AppUser>();
            return passwordhaser.HashPassword(user, password);
        }
    }
}
