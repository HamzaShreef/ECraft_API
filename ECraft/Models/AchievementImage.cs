using ECraft.Constants;
using System.ComponentModel.DataAnnotations;

namespace ECraft.Models
{
	public class AchievementImage:BaseEntity<long>
	{
        public long AchievementId { get; set; }

        public CraftAchievement? Achievement { get; set; }

        [MaxLength(100)]
		public string ImgName { get; set; }


		//Delayed for big loads in case we get into production
		[MaxLength(250)]
		public string? ImgUrl { get; set; }

		[MaxLength(SizeConstants.AchievementImageHeadingMax)]
        public string? Heading { get; set; }

		public int LikesCount { get; set; } = 0;

		public DateTime UploadDate { get; set; } = DateTime.UtcNow;
    }
}
