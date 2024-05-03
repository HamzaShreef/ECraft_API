using ECraft.Models;
using ECraft.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECraft.Data.Configurations.Identity
{
    public class AppUserLoginConfig : IEntityTypeConfiguration<AppUserLogin>
    {
        public void Configure(EntityTypeBuilder<AppUserLogin> builder)
        {
            builder.HasKey(ul => new { ul.LoginProvider, ul.ProviderKey });

            builder.HasOne<AppUser>().WithMany().HasForeignKey(ul => ul.UserId).IsRequired();

            builder.Property(ul => ul.ProviderKey).HasMaxLength(128);
            builder.Property(ul => ul.LoginProvider).HasMaxLength(128);
            builder.Property(ul => ul.ProviderDisplayName).HasMaxLength(500);

            builder.ToTable(nameof(AppUserLogin));
        }
    }
}
