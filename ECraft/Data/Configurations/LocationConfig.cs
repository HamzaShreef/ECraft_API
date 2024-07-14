using ECraft.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECraft.Data.Configurations
{
	public class LocationCountryConfig : IEntityTypeConfiguration<LocationCountry>
	{
		public void Configure(EntityTypeBuilder<LocationCountry> builder)
		{
			builder.HasData(new LocationCountry[]
			{
			new LocationCountry()
			{
				Id = 1,
				CountryName = "Egypt",
				LocalName = "مصر",
				CountryCode = "+20",
				TimeZone = "Africa/Cairo"
			},
			new LocationCountry()
			{
				Id = 2,
				CountryName = "United States",
				CountryCode = "+1",
				TimeZone = "America/New_York"  // You can set this to the most relevant time zone
            }
			});
		}
	}


	public class LocationCityConfig : IEntityTypeConfiguration<LocationCity>
	{
		public void Configure(EntityTypeBuilder<LocationCity> builder)
		{
			builder.HasData(new LocationCity[]
			{
				new LocationCity(){Id=1,CountryId=1,CityName="Alexandria"},
				new LocationCity(){Id=2,CountryId=1,CityName="Mansoura"},
			});

		}
	}
}
