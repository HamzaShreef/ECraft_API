using ECraft.Contracts.Request;
using ECraft.Models;

namespace ECraft.Extensions
{
	public static class CrafterMapping
	{
		public static CrafterProfileRequest GetDto(this CrafterProfileRequest profileRequest, CrafterProfile domainEntity)
		{
			if (domainEntity is null)
				throw new ArgumentNullException(nameof(domainEntity));

			profileRequest.About = domainEntity.About;
			profileRequest.CraftId = domainEntity.CraftId;
			profileRequest.ContactPhone = domainEntity.ContactPhone;
			profileRequest.Title = domainEntity.Title;
			profileRequest.WorkLocation = domainEntity.WorkLocation;


			return profileRequest;
		}

		public static CrafterProfile GetDomainEntity(this CrafterProfileRequest crafterProfileRequest, CrafterProfile persistedProfile)
		{
			persistedProfile.Title = crafterProfileRequest.Title;
			persistedProfile.CraftId= crafterProfileRequest.CraftId;
			persistedProfile.About=crafterProfileRequest.About;
			persistedProfile.ContactPhone = crafterProfileRequest.ContactPhone;
			persistedProfile.WorkLocation = crafterProfileRequest.WorkLocation;

			return persistedProfile;
		}
	}
}
