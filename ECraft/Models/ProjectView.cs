namespace ECraft.Models
{
	public class AchievementView
	{
        public int ViewerId { get; set; }

        public AppUser Viewer { get; set; }

        public int AchievementId { get; set; }

        public CraftAchievement Achievement { get; set; }

        public DateTime ViewDate { get; set; }
    }
}
