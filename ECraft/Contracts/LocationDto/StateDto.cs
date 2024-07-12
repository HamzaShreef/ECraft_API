using ECraft.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ECraft.Contracts.LocationDto
{
	public class StateDto:IDtoMapping<StateDto,LocationRegion>
	{
        public int RegionId { get; set; }

        [MaxLength(100)]
		[MinLength(2)]
        public string RegionName { get; set; }

        public int CountryId { get; set; }

		public LocationRegion GetDomainEntity(out bool successfulMapping, out IdentityError? validationError, LocationRegion? oldInstance=null)
		{
			successfulMapping = true;
			validationError = null;

			if (oldInstance is null)
				return new LocationRegion()
				{
					CountryId = this.CountryId,
					RegionName = this.RegionName,
				};

			oldInstance.CountryId = this.CountryId;
			oldInstance.RegionName = this.RegionName;

			return oldInstance;
		}

		public StateDto GetDto(LocationRegion domainEntity)
		{
			if (domainEntity is null)
				return null;

			this.RegionId= domainEntity.Id;
			this.RegionName= domainEntity.RegionName;
			this.CountryId= domainEntity.CountryId;

			return this;
		}
	}
}
