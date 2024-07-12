using ECraft.Contracts.Request;

namespace ECraft.Contracts.Response
{
	public class CrafterProfileResponse
	{
		public CrafterProfileBasicInfo BasicInfo {  get; set; }

        public string CityName { get; set; }

        public string CountryName { get; set; }

        public string CraftName {  get; set; }

        public DateTime JoinDate { get; set; }

        public int ProjectsCount { get; set; }

        public int SkillsCount { get; set; }

        public int ReviewsCount { get; set; } = 0;

		public double AverageRating { get; set; } = 0;

		public int ViewsCount { get; set; } = 0;
	}
}
