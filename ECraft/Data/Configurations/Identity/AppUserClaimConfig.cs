using ECraft.Models;
using ECraft.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECraft.Data.Configurations.Identity
{
    public class AppUserClaimConfig : IEntityTypeConfiguration<AppUserClaim>
    {
        public void Configure(EntityTypeBuilder<AppUserClaim> builder)
        {
            builder.HasOne<AppUser>().WithMany().HasForeignKey(uc => uc.UserId).IsRequired();
            builder.Property(uc => uc.ClaimType).HasMaxLength(100);
            builder.Property(uc => uc.ClaimValue).HasMaxLength(100);

            builder.ToTable(nameof(AppUserClaim));
        }
    }
}
