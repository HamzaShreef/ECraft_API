using ECraft.Constants;
using System.ComponentModel.DataAnnotations;

namespace ECraft.Models
{
    public class CraftProject : BaseEntity<int>
    {
        [MaxLength(250)]
        public string Title { get; set; }

        [MaxLength(10)]
        public string? RefIdentifier { get; set; }

        [MaxLength(100)]
        public string Slug { get; set; }

        [MaxLength(StringPropertyLengths.ProjectDescriptionMax)]
        public string Description { get; set; }

        public double? Price { get; set; }

        [MaxLength(100)]
        public string? Currency { get; set; }

        [MaxLength(100)]
        public string? IntroImg { get; set; }

        public bool IsPrivate { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? CompletionDate { get; set; }

        public DateTime PublishDate { get; set; } = DateTime.UtcNow;

        public int CraftId { get; set; }

        public Craft Craft { get; set; }

		public CrafterProfile Crafter { get; set; }

        public int CrafterId { get; set; }

        public int LikesCount { get; set; } = 0;

        public int ReviewsCount { get; set; }

        public int ViewCount { get; set; } = 0;


		// New fields
		[MaxLength(1000)]
		public string? MaterialsUsed { get; set; }

		[MaxLength(50)]
		public string Status { get; set; } // E.g., Ongoing, Completed, On Hold

		[MaxLength(100)]
		public string? Location { get; set; }

		[MaxLength(250)]
		public string? VideoUrl { get; set; }

		public ICollection<ProjectTag>? Tags { get; set; }
	}
}
