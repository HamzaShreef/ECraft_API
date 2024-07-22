using ECraft.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECraft.Data.Configurations
{
	public class AchievementTagConfig : IEntityTypeConfiguration<AchievementTag>
	{
		public void Configure(EntityTypeBuilder<AchievementTag> builder)
		{
			builder.HasKey(pt => new {pt.AchievementId,pt.TagId});

			builder.HasOne(pt => pt.Tag).WithMany(t => t.CraftAchievements).HasForeignKey(pt => pt.TagId).OnDelete(DeleteBehavior.NoAction);
			builder.HasOne(pt => pt.Achievement).WithMany(p => p.Tags).HasForeignKey(pt => pt.AchievementId).OnDelete(DeleteBehavior.NoAction);

		}
	}
}
