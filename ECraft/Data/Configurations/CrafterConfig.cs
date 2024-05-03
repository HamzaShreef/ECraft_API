using ECraft.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECraft.Data.Configurations
{
	public class CrafterConfig : IEntityTypeConfiguration<Crafter>
	{
		public void Configure(EntityTypeBuilder<Crafter> builder)
		{
			builder.HasMany(c => c.Skills).WithOne(cs => cs.Crafter).HasForeignKey(cs=>cs.CrafterId); 
			builder.HasMany(c => c.PortofolioProjects).WithOne(cp=>cp.Crafter).HasForeignKey(cp => cp.CrafterId).OnDelete(DeleteBehavior.NoAction);

		}
	}
}
