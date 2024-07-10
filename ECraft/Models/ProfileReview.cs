using System.ComponentModel.DataAnnotations;

namespace ECraft.Models
{
	public class Review : BaseEntity<int>
	{
        public int ReviewerId { get; set; }

        public int? ProfileId { get; set; }

        public CrafterProfile? Profile { get; set; }

        [MaxLength(500)]
        public string Comment { get; set; }

        public byte StarCount { get; set; } = 0;

        public int? ProjectId { get; set; }

        public CraftProject? Project { get; set; }

        public DateTime ReviewDate { get; set; }
    }
}
