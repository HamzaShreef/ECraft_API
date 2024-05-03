using ECraft.Models;
using System.ComponentModel.DataAnnotations;

namespace ECraft.Contracts.LocationDto
{
	public class CountryDto : IDtoMapping<CountryDto, LocationCountry>
	{
		public int CountryId { get; set; }

		[MinLength(2)]
		[MaxLength(50)]
		public string CountryName { get; set; }

		[MaxLength(5)]
		[MinLength(2)]
		public string CountryCode { get; set; }

		public LocationCountry GetDomainEntity(out bool successfulMapping, out string validationErrorMessage, LocationCountry? oldInstance=null)
		{
			successfulMapping = true;
			validationErrorMessage = string.Empty;

			if (oldInstance is null)
				return new LocationCountry()
				{
					CountryName = this.CountryName,
					CountryCode = this.CountryCode,
				};

			oldInstance.CountryName = this.CountryName;
			oldInstance.CountryCode=this.CountryCode;
			return oldInstance;
		}

		public CountryDto GetDto(LocationCountry domainEntity)
		{
			if (domainEntity is null)
				return null;

			this.CountryId = domainEntity.Id;
			this.CountryCode=domainEntity.CountryCode;
			this.CountryName = domainEntity.CountryName;

			return this;
		}
	}
}
