using ECraft.Constants;
using ECraft.Contracts.Request;
using System.ComponentModel.DataAnnotations;

namespace ECraft.Models
{
    public class CraftAchievement : BaseEntity<long>
    {
		[MaxLength(SizeConstants.AchievementDescriptionMax)]
		[MinLength(20)]
		public string Description { get; set; }

		public DateTime? CompletionDate { get; set; }

		public DateTime? StartDate { get; set; }

		public bool IsPrivate { get; set; }

		[MaxLength(250)]
		public string Title { get; set; }

		[MaxLength(10)]
		public string? RefIdentifier { get; set; }

		[MaxLength(100)]
		public string Slug { get; set; }

		public double? Price { get; set; }

		[MaxLength(100)]
		public string? Currency { get; set; }

		// New fields
		[MaxLength(SizeConstants.AchievementMaterialsUsedMax)]
		public string? MaterialsUsed { get; set; }

		[MaxLength(50)]
		public string Status { get; set; } // E.g., Ongoing, Completed, On Hold

		[MaxLength(100)]
		public string? Location { get; set; }

		[MaxLength(250)]
		public string? VideoUrl { get; set; }

		[MaxLength(100)]
        public string? IntroImg { get; set; }

        public DateTime PublishDate { get; set; } = DateTime.UtcNow;

        public int CraftId { get; set; } = 0;

        public Craft Craft { get; set; }

		public CrafterProfile Crafter { get; set; }

        public int CrafterId { get; set; }

        public int LikesCount { get; set; } = 0;

        public int ReviewsCount { get; set; } = 0;

        public int ViewCount { get; set; } = 0;

		public ICollection<AchievementTag>? Tags { get; set; }

		public ICollection<AchievementImage>? Images { get; set; }
	}
}
