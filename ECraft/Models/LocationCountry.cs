using System.ComponentModel.DataAnnotations;

namespace ECraft.Models
{
    public class LocationCountry : BaseEntity<int>
    {
        [MaxLength(100)]
        public string CountryName { get; set; }

        [MaxLength(10)]
        public string CountryCode { get; set; }


		public ICollection<LocationCity> CountryCities { get; set; }

		public ICollection<LocationState> CountryStates { get; set; }
	}
}
