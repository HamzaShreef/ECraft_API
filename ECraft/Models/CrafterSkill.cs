namespace ECraft.Models
{
	public class CrafterSkill
	{
        public int SkillId { get; set; }

        public Skill Skill { get; set; }

        public int CrafterId { get; set; }

        public Crafter Crafter { get; set; }
    }
}
