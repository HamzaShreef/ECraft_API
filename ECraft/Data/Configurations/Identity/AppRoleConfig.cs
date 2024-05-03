using ECraft.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECraft.Data.Configurations.Identity
{
    public class AppRoleConfig : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            builder.HasKey(r => r.Id);

            builder.HasIndex(r => r.NormalizedName).IsUnique().HasDatabaseName("IX_RoleName");
            builder.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

            builder.HasMany<AppUserRole>().WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();
            builder.HasMany<AppRoleClaim>().WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();

            builder.Property(r => r.ConcurrencyStamp).HasMaxLength(50);
            builder.Property(r => r.Name).HasMaxLength(100);
            builder.Property(r => r.NormalizedName).HasMaxLength(100);


            builder.ToTable(nameof(AppRole));

            builder.HasData(
				new AppRole() { Id = 1, Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = Guid.NewGuid().ToString() },
				new AppRole() { Id = 2, Name = "Crafter", NormalizedName = "CRAFTER", ConcurrencyStamp = Guid.NewGuid().ToString() }

				);

        }
    }
}
