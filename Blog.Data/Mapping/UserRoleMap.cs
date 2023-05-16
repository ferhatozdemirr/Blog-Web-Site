using Blog.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.Mapping
{
    public class UserRoleMap : IEntityTypeConfiguration<AppUserRole>
    {
        public void Configure(EntityTypeBuilder<AppUserRole> builder)
        {

            // Primary key
            builder.HasKey(r => new { r.UserId, r.RoleId });

            // Maps to the AspNetUserRoles table
            builder.ToTable("AspNetUserRoles");

            builder.HasData(new AppUserRole
            {
                UserId = Guid.Parse("45CB1797-8868-4C0D-846D-D7E5C3E4BB26"),
                RoleId = Guid.Parse("BA4D4699-25C1-48DA-8CEF-69E6A1942723")
            }, new AppUserRole
            {
                UserId = Guid.Parse("66DC5149-6B91-462C-8EBC-2CD2C18C7D35"),
                RoleId = Guid.Parse("F1AB3D99-E78A-4538-9719-551F9A8C4678")
            });
        }
    }
}
