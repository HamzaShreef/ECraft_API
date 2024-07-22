using ECraft.Contracts.LocationDto;
using ECraft.Models;
using Microsoft.AspNetCore.Identity;

namespace ECraft.Extensions
{
	public static class LocationMapping
	{
		//City Mapping
		public static LocationCity GetDomainEntity(this CityDto cityDto, LocationCity? oldInstance = null)
		{
			if (oldInstance is null)
			{
				oldInstance = new LocationCity();
			}

			oldInstance.CityName = cityDto.CityName;
			oldInstance.LocalName = cityDto.LocalName;
			oldInstance.RegionId = cityDto.RegionId;
			oldInstance.CountryId = cityDto.CountryId;


			return oldInstance;
		}

		public static CityDto GetDto(this CityDto cityDto, LocationCity domainEntity)
		{
			if (domainEntity is null)
				return null;

			cityDto.CityId = domainEntity.Id;
			cityDto.CityName = domainEntity.CityName;
			cityDto.LocalName = domainEntity.LocalName;
			cityDto.RegionId = domainEntity.RegionId;
			cityDto.CountryId = domainEntity.CountryId;

			return cityDto;
		}

		//Country Mapping
		public static LocationCountry GetDomainEntity(this CountryDto countryDto, LocationCountry? oldInstance = null)
		{

			if (oldInstance is null)
			{
				oldInstance=new LocationCountry();
			}

			oldInstance.CountryName = countryDto.CountryName;
			oldInstance.CountryCode = countryDto.CountryCode;
			oldInstance.LocalName = countryDto.LocalName;
			return oldInstance;
		}

		public static CountryDto GetDto(this CountryDto countryDto,LocationCountry domainEntity)
		{
			if (domainEntity is null)
				return null;

			countryDto.CountryId = domainEntity.Id;
			countryDto.CountryCode = domainEntity.CountryCode;
			countryDto.CountryName = domainEntity.CountryName;
			countryDto.LocalName = domainEntity.LocalName;

			return countryDto;
		}

		//State Or Region Mapping
		public static LocationRegion GetDomainEntity(this RegionDto regionDto, LocationRegion? oldInstance = null)
		{
			if (oldInstance is null)
			{
				oldInstance = new LocationRegion();
			}

			oldInstance.CountryId = regionDto.CountryId;
			oldInstance.RegionName = regionDto.RegionName;
			oldInstance.LocalName = regionDto.LocalName;

			return oldInstance;
		}

		public static RegionDto GetDto(this RegionDto regionDto,LocationRegion domainEntity)
		{
			if (domainEntity is null)
				return null;

			regionDto.RegionId = domainEntity.Id;
			regionDto.RegionName = domainEntity.RegionName;
			regionDto.LocalName = domainEntity.LocalName;
			regionDto.CountryId = domainEntity.CountryId;

			return regionDto;
		}
	}
}
