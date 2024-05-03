using ECraft.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECraft.Data.Configurations
{
	public class SkillConfig : IEntityTypeConfiguration<Skill>
	{
		public void Configure(EntityTypeBuilder<Skill> builder)
		{
			builder.HasMany(sk => sk.Crafters).WithOne(cs => cs.Skill).HasForeignKey(cs => cs.SkillId).IsRequired();
		}
	}
}
