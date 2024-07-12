using System.ComponentModel.DataAnnotations;

namespace ECraft.Models
{
    public class LocationRegion : BaseEntity<int>
    {
        [MaxLength(100)]
        public string RegionName { get; set; }

        [MaxLength(100)]
        public string? LocalName { get; set; }

        public int CountryId { get; set; }

        public LocationCountry Country { get; set; }

        public ICollection<LocationCity> RegionCities { get; set; }
    }
}
