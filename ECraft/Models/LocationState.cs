using System.ComponentModel.DataAnnotations;

namespace ECraft.Models
{
    public class LocationState : BaseEntity<int>
    {
        [MaxLength(100)]
        public string? StateName { get; set; }

        [MaxLength(100)]
        public string LocalName { get; set; }

        public int CountryId { get; set; }

        public LocationCountry Country { get; set; }

        public ICollection<LocationCity> StateCities { get; set; }
    }
}
