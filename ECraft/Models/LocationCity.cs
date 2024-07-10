using System.ComponentModel.DataAnnotations;

namespace ECraft.Models
{
    public class LocationCity : BaseEntity<int>
    {
        [MaxLength(100)]
        public string CityName { get; set; }

        [MaxLength(100)]
        public string? LocalName { get; set; }

        public int? StateId { get; set; }

        public LocationState? State { get; set; }

        public int CountryId { get; set; }

        public LocationCountry Country { get; set; }

        public int UsersCount { get; set; } = 0;

        public int CraftersCount { get; set; } = 0;
    }
}
