using ECraft.Contracts.Request;
using ECraft.Contracts.Response;
using ECraft.Models;

namespace ECraft.Extensions
{
	public static class CrafterMapping
	{
		public static CrafterProfileBasicInfo GetDto(this CrafterProfileBasicInfo profileRequest, CrafterProfile domainEntity)
		{
			if (profileRequest == null)
			{
				profileRequest = new CrafterProfileBasicInfo();
			}
			if (domainEntity is null)
				throw new ArgumentNullException(nameof(domainEntity));

			profileRequest.CraftId = domainEntity.CraftId;
			profileRequest.ContactPhone = domainEntity.ContactPhone;
			profileRequest.Title = domainEntity.Title;
			profileRequest.About = domainEntity.About;
			profileRequest.WorkLocation = domainEntity.WorkLocation;


			return profileRequest;
		}

		public static CrafterProfile GetDomainEntity(this CrafterProfileBasicInfo crafterProfileRequest, CrafterProfile persistedProfile)
		{
			persistedProfile.CraftId= crafterProfileRequest.CraftId;
			persistedProfile.ContactPhone = crafterProfileRequest.ContactPhone;
			persistedProfile.Title = crafterProfileRequest.Title;
			persistedProfile.About=crafterProfileRequest.About;
			persistedProfile.WorkLocation = crafterProfileRequest.WorkLocation;

			return persistedProfile;
		}

		public static CrafterProfileResponse GetResponseDto(this CrafterProfileResponse profileInfo, CrafterProfile domainEntity)
		{
			if (profileInfo == null)
			{
				profileInfo = new CrafterProfileResponse();
			}
			if (domainEntity is null)
				throw new ArgumentNullException(nameof(domainEntity));

			profileInfo.BasicInfo = profileInfo.BasicInfo.GetDto(domainEntity);

			profileInfo.ProjectsCount = domainEntity.ProjectsCount;
			profileInfo.ViewsCount = domainEntity.ViewsCount;
			profileInfo.SkillsCount = domainEntity.SkillsCount;
			profileInfo.AverageRating = domainEntity.AverageRating;
			profileInfo.JoinDate = domainEntity.JoinDate;
			profileInfo.ReviewsCount = domainEntity.ReviewsCount;


			return profileInfo;
		}

	}
}
