using ECraft.Models;
using ECraft.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECraft.Data.Configurations.Identity
{
    public class AppUserConfig : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            //builder.UseTptMappingStrategy();

            builder.HasIndex(u => u.NormalizedUserName).IsUnique().HasDatabaseName("IX_UserName");
            builder.HasIndex(u => u.NormalizedEmail).IsUnique().HasDatabaseName("IX_Email");


            builder.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

            builder.HasMany<AppUserRole>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
            builder.HasMany<AppUserClaim>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
            builder.HasMany<AppUserToken>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();
            builder.HasMany<AppUserLogin>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();

            builder.Property(u => u.Email).HasMaxLength(256);
            builder.Property(u => u.PhoneNumber).HasMaxLength(20);
            builder.Property(u => u.NormalizedEmail).HasMaxLength(256);
            builder.Property(u => u.UserName).HasMaxLength(256);
            builder.Property(u => u.NormalizedUserName).HasMaxLength(256);
            builder.Property(u => u.SecurityStamp).HasMaxLength(128);
            builder.Property(u => u.ConcurrencyStamp).HasMaxLength(50);
            builder.Property(u => u.PasswordHash).HasMaxLength(256);

            builder.HasMany(u => u.RefreshTokens).WithOne(tk => tk.User).HasForeignKey(tk => tk.UserId).IsRequired();

            builder.ToTable(nameof(AppUser));

        }
    }
}
