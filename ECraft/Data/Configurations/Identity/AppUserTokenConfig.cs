using ECraft.Models;
using ECraft.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECraft.Data.Configurations.Identity
{
    public class AppUserTokenConfig : IEntityTypeConfiguration<AppUserToken>
    {
        public void Configure(EntityTypeBuilder<AppUserToken> builder)
        {
            builder.HasKey(ut => new { ut.UserId, ut.LoginProvider, ut.Name });

            builder.HasOne<AppUser>().WithMany().HasForeignKey(ut => ut.UserId).IsRequired();

            builder.Property(ut => ut.LoginProvider).HasMaxLength(128);
            builder.Property(ut => ut.Name).HasMaxLength(128);
            builder.Property(ut => ut.Value).HasMaxLength(1000);

            builder.ToTable(nameof(AppUserToken));
        }
    }
}
