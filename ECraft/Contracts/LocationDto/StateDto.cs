using ECraft.Models;
using System.ComponentModel.DataAnnotations;

namespace ECraft.Contracts.LocationDto
{
	public class StateDto:IDtoMapping<StateDto,LocationState>
	{
        public int StateId { get; set; }

        [MaxLength(100)]
		[MinLength(2)]
        public string StateName { get; set; }

        public int CountryId { get; set; }

		public LocationState GetDomainEntity(out bool successfulMapping, out string validationErrorMessage, LocationState? oldInstance=null)
		{
			successfulMapping = true;
			validationErrorMessage=string.Empty;
			if (oldInstance is null)
				return new LocationState()
				{
					CountryId = this.CountryId,
					StateName = this.StateName,
				};

			oldInstance.CountryId = this.CountryId;
			oldInstance.StateName = this.StateName;

			return oldInstance;
		}

		public StateDto GetDto(LocationState domainEntity)
		{
			if (domainEntity is null)
				return null;

			this.StateId= domainEntity.Id;
			this.StateName= domainEntity.StateName;
			this.CountryId= domainEntity.CountryId;

			return this;
		}
	}
}
