namespace ECraft.Contracts.LocationDto
{
	public class RegionWithCitiesDto:RegionDto
	{
        public IEnumerable<CityDto>? RegionCities { get; set; }

        public int CitiesCount { get; set; } = 0;
    }
}
