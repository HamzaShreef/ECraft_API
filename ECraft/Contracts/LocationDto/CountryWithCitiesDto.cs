using ECraft.Models;

namespace ECraft.Contracts.LocationDto
{
	public class CountryWithCitiesDto : CountryDto
	{
		public IEnumerable<CityDto>? CountryCities { get; set; }

        public int citiesCount { get; set; }

        public IEnumerable<StateDto>? CountryRegions { get; set; }

        public int statesCount { get; set; }

    }
}
