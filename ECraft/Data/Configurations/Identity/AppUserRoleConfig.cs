using ECraft.Models;
using ECraft.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECraft.Data.Configurations.Identity
{
    public class AppUserRoleConfig : IEntityTypeConfiguration<AppUserRole>
    {
        public void Configure(EntityTypeBuilder<AppUserRole> builder)
        {
            builder.HasKey(ur => new { ur.UserId, ur.RoleId });

            builder.HasOne<AppUser>().WithMany().HasForeignKey(ur => ur.UserId).IsRequired();
            builder.HasOne<AppRole>().WithMany().HasForeignKey(ur => ur.RoleId).IsRequired();

            builder.ToTable(nameof(AppUserRole));
        }
    }
}
