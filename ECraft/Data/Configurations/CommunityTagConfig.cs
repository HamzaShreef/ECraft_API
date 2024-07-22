using ECraft.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECraft.Data.Configurations
{
	public class CommunityTagConfig : IEntityTypeConfiguration<CommunityTag>
	{
		public void Configure(EntityTypeBuilder<CommunityTag> builder)
		{
			builder.HasMany(ct => ct.CraftAchievements).WithOne(pt => pt.Tag).HasForeignKey(pt => pt.TagId);
		}
	}
}
