using ECraft.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace ECraft.Data.Configurations
{
	public class ProfileVeiwConfig : IEntityTypeConfiguration<ProfileView>
	{
		public void Configure(EntityTypeBuilder<ProfileView> builder)
		{
			builder.HasKey(pv => new { pv.ProfileId, pv.ViewerId });

			builder.HasIndex(pv => pv.ProfileId).IsUnique(false);

			builder.HasOne(pv => pv.Profile).WithMany(profile => profile.ProfileViews).HasForeignKey(pv=>pv.ProfileId);

			builder.HasOne(pv => pv.Viewer).WithMany().HasForeignKey(pv => pv.ViewerId).IsRequired(false);
		}
	}
}
