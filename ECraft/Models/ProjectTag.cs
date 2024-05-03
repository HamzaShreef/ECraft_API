namespace ECraft.Models
{
	public class ProjectTag
	{
        public int TagId { get; set; }

        public CommunityTag Tag { get; set; }

        public int ProjectId { get; set; }

        public CraftProject Project { get; set; }
    }
}
