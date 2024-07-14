using ECraft.Contracts.LocationDto;
using ECraft.Models;
using Microsoft.AspNetCore.Identity;

namespace ECraft.Extensions
{
	public static class LocationMapping
	{
		//City Mapping
		public static LocationCity GetDomainEntity(this CityDto cityDto,out bool successfulMapping, LocationCity? oldInstance = null)
		{
			successfulMapping = true;

			if (oldInstance is null)
			{
				return new LocationCity()
				{
					CityName = cityDto.CityName,
					CountryId = cityDto.CountryId,
					RegionId = cityDto.RegionId,
				};
			}
			else
			{
				oldInstance.CityName = cityDto.CityName;
				oldInstance.RegionId = cityDto.RegionId;
				oldInstance.CountryId = cityDto.CountryId;
				return oldInstance;
			}

		}

		public static CityDto GetDto(this CityDto cityDto, LocationCity domainEntity)
		{
			if (domainEntity is null)
				return null;

			cityDto.CityId = domainEntity.Id;
			cityDto.CityName = domainEntity.CityName;
			cityDto.RegionId = domainEntity.RegionId;
			cityDto.CountryId = domainEntity.CountryId;

			return cityDto;
		}

		//Country Mapping
		public static LocationCountry GetDomainEntity(this CountryDto countryDto, out bool successfulMapping, LocationCountry? oldInstance = null)
		{
			successfulMapping = true;

			if (oldInstance is null)
				return new LocationCountry()
				{
					CountryName = countryDto.CountryName,
					CountryCode = countryDto.CountryCode,
				};

			oldInstance.CountryName = countryDto.CountryName;
			oldInstance.CountryCode = countryDto.CountryCode;
			return oldInstance;
		}

		public static CountryDto GetDto(this CountryDto countryDto,LocationCountry domainEntity)
		{
			if (domainEntity is null)
				return null;

			countryDto.CountryId = domainEntity.Id;
			countryDto.CountryCode = domainEntity.CountryCode;
			countryDto.CountryName = domainEntity.CountryName;

			return countryDto;
		}

		//State Or Region Mapping
		public static LocationRegion GetDomainEntity(this StateDto regionDto,out bool successfulMapping, LocationRegion? oldInstance = null)
		{
			successfulMapping = true;

			if (oldInstance is null)
				return new LocationRegion()
				{
					CountryId = regionDto.CountryId,
					RegionName = regionDto.RegionName,
				};

			oldInstance.CountryId = regionDto.CountryId;
			oldInstance.RegionName = regionDto.RegionName;

			return oldInstance;
		}

		public static StateDto GetDto(this StateDto regionDto,LocationRegion domainEntity)
		{
			if (domainEntity is null)
				return null;

			regionDto.RegionId = domainEntity.Id;
			regionDto.RegionName = domainEntity.RegionName;
			regionDto.CountryId = domainEntity.CountryId;

			return regionDto;
		}
	}
}
