using ECraft.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECraft.Models
{
	public class CrafterProfile:BaseEntity<int>
	{

        [ForeignKey(nameof(UserRecord))]
        public int UserId { get; set; }


        public AppUser UserRecord { get; set; }

        [MaxLength(20)]
        public string ContactPhone { get; set; }

        [MaxLength(StringPropertyLengths.CrafterTitleMax)]
        public string Title { get; set; }

        [MaxLength(StringPropertyLengths.AboutCrafterMax)]
        public string? About { get; set; }

        [MaxLength(250)]
        public string? WorkLocation { get; set; }

        public int CraftId { get; set; }

        public int ReviewsCount { get; set; } = 0;

        public double AverageRating { get; set; } = 0;

        public int ViewsCount { get; set; } = 0;

        public ExperienceLevel ExperienceLevel { get; set; }

        public Craft Craft { get; set; }

		public DateTime JoinDate { get; set; }

        public int SkillsCount { get; set; }

        public ICollection<CraftProject> PortofolioProjects { get; set; }

        public ICollection<CrafterSkill> Skills { get; set; }

        public ICollection<ProfileView> ProfileViews { get; set; }
    }
}
