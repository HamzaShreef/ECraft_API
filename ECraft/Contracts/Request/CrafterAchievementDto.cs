using ECraft.Constants;
using System.ComponentModel.DataAnnotations;

namespace ECraft.Contracts.Request
{

	public class CrafterAchievementDto
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

        //public IFormFile? IntroImg { get; set; }
    }
}
