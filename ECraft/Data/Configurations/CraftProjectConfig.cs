using ECraft.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECraft.Data.Configurations
{
	public class CraftProjectConfig : IEntityTypeConfiguration<CraftProject>
	{
		public void Configure(EntityTypeBuilder<CraftProject> builder)
		{
			builder.HasMany(p => p.Tags).WithOne(cp => cp.Project).HasForeignKey(cp=>cp.ProjectId);
		}
	}
}
