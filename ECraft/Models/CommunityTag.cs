using System.ComponentModel.DataAnnotations;

namespace ECraft.Models
{
    public class CommunityTag : BaseEntity<int>
    {
        [MaxLength(50)]
        public string TagString { get; set; }

        public int PublicationsCount { get; set; }

        public ICollection<ProjectTag>? CraftProjects { get; set; }

    }
}
