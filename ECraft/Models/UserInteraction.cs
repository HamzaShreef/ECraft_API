namespace ECraft.Models
{
	public class UserInteraction : BaseEntity<long>
	{
		public int InteractorId { get; set; }

		public AppUser Interactor { get; set; }

        public int? InteractionType { get; set; }

		public DateTime IDate { get; set; }

		public long? AchievementId { get; set; }

		public CraftAchievement? Achievement { get; set; }

        public long? ImgId { get; set; }

		public AchievementImage? Img { get; set; }

		public int? ProfileId { get; set; }

		public CrafterProfile? Profile { get; set; }
	}

}
