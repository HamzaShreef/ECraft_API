using System.ComponentModel.DataAnnotations;

namespace ECraft.Models
{
	public class Crafter : AppUser
	{
        public Crafter(string firstName,string lastName,int craftId,string contactPhone)
            :base(firstName,lastName)
        {
            CraftId = craftId;
            ContactPhone= contactPhone;
        }

        [MaxLength(20)]
        public string ContactPhone { get; set; }

        [MaxLength(500)]
        public string? About { get; set; }

        public int CraftId { get; set; }

        public int ReviewsCount { get; set; } = 0;

        public int ViewsCount { get; set; } = 0;

        public Craft Craft { get; set; }

		public DateTime JoinDate { get; set; }

		public ICollection<CraftProject> PortofolioProjects { get; set; }

        public ICollection<CrafterSkill> Skills { get; set; }

        public ICollection<ProfileView> ProfileViews { get; set; }
    }
}
