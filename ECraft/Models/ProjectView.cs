namespace ECraft.Models
{
	public class ProjectView
	{
        public int ViewerId { get; set; }

        public AppUser Viewer { get; set; }

        public int ProjectId { get; set; }

        public CraftProject Project { get; set; }

        public DateTime ViewDate { get; set; }
    }
}
