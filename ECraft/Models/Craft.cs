using System.ComponentModel.DataAnnotations;

namespace ECraft.Models
{
	public class Craft : BaseEntity<int>
	{
		[MaxLength(50)]
		public string Title { get; set; }

		[MaxLength(500)]
		public string Description { get; set; }

		public DateTime CreationDate { get; private set; } = DateTime.UtcNow;

		public int CraftersCount { get; set; }

        [MaxLength(100)]
		public string? Icon { get; set; }

		public ICollection<CrafterProfile>? CraftProfessionals { get; set; }
	}
}
