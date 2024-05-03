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

		[MaxLength(50)]
		public string? Icon { get; set; }

		public ICollection<Crafter>? CraftProfessionals { get; set; }
	}
}
