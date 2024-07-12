using System.ComponentModel.DataAnnotations;

namespace ECraft.Models
{
    public class LocationCountry : BaseEntity<int>
    {
        [MaxLength(100)]
        public string CountryName { get; set; }

        [MaxLength(100)]
        public string? LocalName { get; set; }

        [MaxLength(10)]
        public string CountryCode { get; set; }

		[MaxLength(100)]
		public string TimeZone { get; set; }  // e.g., "America/New_York"

		public ICollection<LocationCity> CountryCities { get; set; }

		public ICollection<LocationRegion> CountryStates { get; set; }
	}
}
