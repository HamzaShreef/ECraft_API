using System.ComponentModel.DataAnnotations;

namespace ECraft.Models
{
    public class Skill : BaseEntity<int>
    {
        [MaxLength(100)]
        public string Name { get; set; }

        public int CraftId { get; set; }

        public Craft Craft { get; set; }

        public DateTime CreationDate { get; set; }

        public int CreatorId { get; set; }

        public AppUser Creator { get; set; }

        public bool Approved { get; set; }    

        [MaxLength(50)]
        public string? Icon { get; set; }

        public ICollection<CrafterSkill>? Crafters { get; set; }

        public int CraftersCount { get; set; } = 0;
    }
}
