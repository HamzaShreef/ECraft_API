using ECraft.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECraft.Data.Configurations.Identity
{
    public class AppRoleClaimConfig : IEntityTypeConfiguration<AppRoleClaim>
    {
        public void Configure(EntityTypeBuilder<AppRoleClaim> builder)
        {
            builder.HasOne<AppRole>().WithMany().HasForeignKey(rc => rc.RoleId).IsRequired();

            builder.Property(rc => rc.ClaimType).HasMaxLength(100);
            builder.Property(rc => rc.ClaimValue).HasMaxLength(100);

            builder.ToTable(nameof(AppRoleClaim));
        }
    }
}
