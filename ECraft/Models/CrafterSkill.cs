namespace ECraft.Models
{
	public class CrafterSkill
	{
        public int SkillId { get; set; }

        public Skill Skill { get; set; }

        public int CrafterId { get; set; }

        public CrafterProfile Crafter { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.UtcNow;

        public ExperienceLevel ExperienceLevel { get; set; }
    }

    public enum ExperienceLevel : int
    {
        LessOne = 1,
        OneToThree,
        ThreeToFive,
        FiveToSeven
    }
}
