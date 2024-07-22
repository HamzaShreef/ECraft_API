using System.ComponentModel.DataAnnotations;

namespace ECraft.Models
{
	public class CrafterReview : BaseEntity<long>
	{
        public int ReviewerId { get; set; }

        public int? ProfileId { get; set; }

        public CrafterProfile? Profile { get; set; }

        [MaxLength(500)]
        public string Comment { get; set; }

        public byte StarCount { get; set; } = 0;

        public long? AchievementId { get; set; }

        public int LikesCount { get; set; } = 0;

        public CraftAchievement? Achievement { get; set; }

        public DateTime ReviewDate { get; set; }
    }
}
