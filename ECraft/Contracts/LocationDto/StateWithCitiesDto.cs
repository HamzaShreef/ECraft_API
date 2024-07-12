namespace ECraft.Contracts.LocationDto
{
	public class StateWithCitiesDto:StateDto
	{
        public IEnumerable<CityDto>? RegionCities { get; set; }

        public int CitiesCount { get; set; } = 0;
    }
}
