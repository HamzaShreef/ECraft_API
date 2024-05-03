using ECraft.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECraft.Data.Configurations
{
	public class ProjectTagConfig : IEntityTypeConfiguration<ProjectTag>
	{
		public void Configure(EntityTypeBuilder<ProjectTag> builder)
		{
			builder.HasKey(pt => new {pt.ProjectId,pt.TagId});

			builder.HasOne(pt => pt.Tag).WithMany(t => t.CraftProjects).HasForeignKey(pt => pt.TagId).OnDelete(DeleteBehavior.NoAction);
			builder.HasOne(pt => pt.Project).WithMany(p => p.Tags).HasForeignKey(pt => pt.ProjectId).OnDelete(DeleteBehavior.NoAction);

		}
	}
}
