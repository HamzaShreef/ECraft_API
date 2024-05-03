using System.ComponentModel.DataAnnotations;

namespace ECraft.Models
{
    public class CraftProject : BaseEntity<int>
    {
        [MaxLength(250)]
        public string Title { get; set; }

        [MaxLength(10)]
        public string RefIdentifier { get; set; }

        [MaxLength(100)]
        public string Slug { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [MaxLength(100)]
        public string IntroImg { get; set; }

        public bool PrivateToOwner { get; set; }

        public DateTime CompletionDate { get; set; }

        public int CraftId { get; set; }

        public Craft Craft { get; set; }

		public Crafter Crafter { get; set; }

        public int CrafterId { get; set; }

        public int LikesCount { get; set; } = 0;

        public int ViewCount { get; set; } = 0;

		public ICollection<ProjectTag>? Tags { get; set; }
    }
}
