namespace ECraft.Models
{
	public class UserInteraction : BaseEntity<int>
	{
		public int InteractorId { get; set; }

		public AppUser Interactor { get; set; }

        public int? InteractionType { get; set; }

		public DateTime IDate { get; set; }

		public int? ProjectId { get; set; }

		public CraftProject? Project { get; set; }

        public int? ImgId { get; set; }

		public ProjectImage? Img { get; set; }

		public int? ProfileId { get; set; }

		public CrafterProfile? Profile { get; set; }
	}

}
