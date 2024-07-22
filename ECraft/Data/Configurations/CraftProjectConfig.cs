using ECraft.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECraft.Data.Configurations
{
	public class CraftAchievementConfig : IEntityTypeConfiguration<CraftAchievement>
	{
		public void Configure(EntityTypeBuilder<CraftAchievement> builder)
		{
			builder.HasMany(p => p.Tags).WithOne(cp => cp.Achievement).HasForeignKey(cp => cp.AchievementId);

			builder.HasMany(ca => ca.Images).WithOne(img => img.Achievement).HasForeignKey(img => img.AchievementId);
		}
	}
}
