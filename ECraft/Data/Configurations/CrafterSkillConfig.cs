using ECraft.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECraft.Data.Configurations
{
	public class CrafterSkillConfig : IEntityTypeConfiguration<CrafterSkill>
	{
		public void Configure(EntityTypeBuilder<CrafterSkill> builder)
		{
			builder.HasKey(entity=>new {entity.CrafterId, entity.SkillId});

			builder.HasOne(cs => cs.Skill).WithMany(sk => sk.Crafters).HasForeignKey(cs => cs.SkillId).OnDelete(DeleteBehavior.NoAction);
			builder.HasOne(cs => cs.Crafter).WithMany(c => c.Skills).HasForeignKey(cs => cs.CrafterId).OnDelete(DeleteBehavior.NoAction);
		}
	}
}
