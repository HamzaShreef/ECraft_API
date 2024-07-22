using System.ComponentModel.DataAnnotations;

namespace ECraft.Models
{
    public class AchievementTag
    {
        public int TagId { get; set; }

        public CommunityTag Tag { get; set; }

        public long AchievementId { get; set; }

        public CraftAchievement Achievement { get; set; }
    }
}
