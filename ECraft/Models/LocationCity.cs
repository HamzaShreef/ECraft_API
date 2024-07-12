using System.ComponentModel.DataAnnotations;

namespace ECraft.Models
{
    public class LocationCity : BaseEntity<int>
    {
        [MaxLength(100)]
        public string CityName { get; set; }

        [MaxLength(100)]
        public string? LocalName { get; set; }

        public int? RegionId { get; set; }

		[MaxLength(50)]
		public string? TimeZone { get; set; }  // e.g., "America/New_York"

		public LocationRegion? State { get; set; }

        public int CountryId { get; set; }

        public LocationCountry Country { get; set; }

        public int UsersCount { get; set; } = 0;

        public int CraftersCount { get; set; } = 0;
    }
}
