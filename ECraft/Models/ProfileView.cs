namespace ECraft.Models
{
	public class ProfileView
	{
        public int ViewerId { get; set; }

		public int ProfileId { get; set; }

        public AppUser Viewer { get; set; }

        public Crafter Profile { get; set; }

        public DateTime ViewDate { get; set; }

    }
}
