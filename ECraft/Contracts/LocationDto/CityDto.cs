using ECraft.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ECraft.Contracts.LocationDto
{
	public class CityDto : IDtoMapping<CityDto, LocationCity>
	{
		public int CityId { get; set; }

		[MinLength(2)]
		[MaxLength(100)]
		public string CityName { get; set; }

		public int CountryId { get; set; }

		public int? StateId { get; set; }

		public LocationCity GetDomainEntity(out bool successfulMapping, out IdentityError? validationError, LocationCity? oldInstance=null)
		{
			successfulMapping = true;
			validationError = null;

			if (oldInstance is null)
			{
				return new LocationCity()
				{
					CityName = this.CityName,
					CountryId = this.CountryId,
					StateId = this.StateId,
				};
			}
			else
			{
				oldInstance.CityName=this.CityName;
				oldInstance.StateId=this.StateId;
				oldInstance.CountryId=this.CountryId;
				return oldInstance;
			}

		}

		public CityDto GetDto(LocationCity domainEntity)
		{
			if (domainEntity is null)
				return null;

			this.CityId = domainEntity.Id;
			this.CityName = domainEntity.CityName;
			this.StateId = domainEntity.StateId;
			this.CountryId = domainEntity.CountryId;

			return this;
		}
	}
}
