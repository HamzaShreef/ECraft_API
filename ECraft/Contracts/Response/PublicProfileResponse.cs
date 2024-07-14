using ECraft.Contracts.Request;

namespace ECraft.Contracts.Response
{
	public class PublicProfileResponse
	{
		public CrafterProfileBasicInfo BasicInfo {  get; set; }

        public int ProfileId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? UserName { get; set; }

        public DateTime JoinDate { get; set; }

        public int ProjectsCount { get; set; }

        public int SkillsCount { get; set; }

        public int ReviewsCount { get; set; } = 0;

		public double AverageRating { get; set; } = 0;

        public int LikesCount { get; set; } = 0;

        public string? ProfileImg { get; set; }

        public bool IsMale { get; set; }

        //Navigation Scalars
        public int CityId { get; set; }

        public string CityName { get; set; }

        public int CountryId { get; set; }

        public string CountryName { get; set; }

        public int? RegionId { get; set; }

        public string RegionName { get; set; }

        public string CraftTitle {  get; set; }

        public string? CraftIcon { get; set; }
    }
}
