﻿using Azure;
using ECraft.Contracts.Request;
using ECraft.Contracts.Response;
using ECraft.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
			profileRequest.LocationLongitude = domainEntity.Longitude;
			profileRequest.LocationLatitude = domainEntity.Latitude;


			return profileRequest;
		}

		public static CrafterProfile GetDomainEntity(this CrafterProfileBasicInfo crafterProfileRequest, CrafterProfile persistedProfile)
		{
			if (persistedProfile is null)
			{
				persistedProfile = new CrafterProfile();
			}

			persistedProfile.CraftId= crafterProfileRequest.CraftId;
			persistedProfile.ContactPhone = crafterProfileRequest.ContactPhone;
			persistedProfile.Title = crafterProfileRequest.Title;
			persistedProfile.About=crafterProfileRequest.About;
			persistedProfile.WorkLocation = crafterProfileRequest.WorkLocation;
			persistedProfile.Latitude = crafterProfileRequest.LocationLatitude;
			persistedProfile.Longitude = crafterProfileRequest.LocationLongitude;

			return persistedProfile;
		}

		public static PublicProfileResponse GetResponseDto(this PublicProfileResponse profileInfo, CrafterProfile domainEntity,Craft craft=null,UserProfileResponse userProfile=null)
		{
			if (profileInfo == null)
			{
				profileInfo = new PublicProfileResponse();
			}
			if (domainEntity is null)
				throw new ArgumentNullException(nameof(domainEntity));

			profileInfo.BasicInfo = profileInfo.BasicInfo.GetDto(domainEntity);

			profileInfo.ProjectsCount = domainEntity.ProjectsCount;
			profileInfo.ProfileId = domainEntity.Id;
			profileInfo.SkillsCount = domainEntity.SkillsCount;
			profileInfo.AverageRating = domainEntity.AverageRating;
			profileInfo.JoinDate = domainEntity.JoinDate;
			profileInfo.ReviewsCount = domainEntity.ReviewsCount;
			profileInfo.LikesCount = domainEntity.LikesCount;

			if(craft is not null)
			{
				profileInfo.CraftTitle = craft.Title;
				profileInfo.CraftIcon = craft.Icon;
			}

			if(userProfile is not null)
			{
				profileInfo.FirstName = userProfile.FirstName;
				profileInfo.LastName = userProfile.LastName;
				profileInfo.UserName = userProfile.UserName;
				profileInfo.ProfileImg = userProfile.Picture;
				profileInfo.IsMale = userProfile.isMale;
			}


			return profileInfo;
		}

		public static PrivateProfileResponse GetPrivateInfo(this PrivateProfileResponse profileInfo,PublicProfileResponse publicInfo,CrafterProfile domainEntity)
		{
			//profileInfo.PublicInfo=profileInfo.PublicInfo.GetResponseDto(domainEntity);

			profileInfo.PublicInfo = publicInfo;
			profileInfo.ViewsCount=domainEntity.ViewsCount;

			return profileInfo;
		}
	}
}
