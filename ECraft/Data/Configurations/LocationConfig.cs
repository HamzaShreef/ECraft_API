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
				new LocationCountry(){Id=1, CountryName="Egypt",CountryCode="+20"},
				new LocationCountry(){ Id=2,CountryName="United States",CountryCode="+1"},
			});

		}
	}
}
