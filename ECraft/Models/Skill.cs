using System.ComponentModel.DataAnnotations;

namespace ECraft.Models
{
    public class Skill : BaseEntity<int>
    {
        [MaxLength(100)]
        public string Name { get; set; }

        public int CategoryId { get; set; }

        public Craft Category { get; set; }

        [MaxLength(50)]
        public string? Icon { get; set; }

        public ICollection<CrafterSkill>? Crafters { get; set; }

        public int CraftersCount { get; set; } = 0;
    }
}
