using System.ComponentModel.DataAnnotations;

namespace ECraft.Models
{
    public class LocationState : BaseEntity<int>
    {
        [MaxLength(250)]
        public string StateName { get; set; }

        public int CountryId { get; set; }

        public LocationCountry Country { get; set; }

        public ICollection<LocationCity> StateCities { get; set; }
    }
}
