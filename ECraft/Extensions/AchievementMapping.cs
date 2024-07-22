using ECraft.Contracts.Request;
using ECraft.Contracts.Response;
using ECraft.Models;
using ECraft.Services;

namespace ECraft.Extensions
{
	public static class AchievementMapping
	{

		public static CraftAchievement GetDomainEntity(this CrafterAchievementDto achievementDto, CraftAchievement? persistedEntity=null)
		{
			if (persistedEntity is null)
			{
				persistedEntity = new CraftAchievement();
			}


			persistedEntity.Description = achievementDto.Description;
			persistedEntity.CompletionDate = achievementDto.CompletionDate;
			persistedEntity.StartDate = achievementDto.StartDate;
			persistedEntity.IsPrivate = achievementDto.IsPrivate;
			persistedEntity.Title = achievementDto.Title;
			persistedEntity.RefIdentifier = achievementDto.RefIdentifier;
			persistedEntity.Slug = achievementDto.Slug;
			persistedEntity.Price = achievementDto.Price;
			persistedEntity.Currency = achievementDto.Currency;
			persistedEntity.MaterialsUsed = achievementDto.MaterialsUsed;
			persistedEntity.Status = achievementDto.Status;
			persistedEntity.Location = achievementDto.Location;
			persistedEntity.VideoUrl = achievementDto.VideoUrl;


			return persistedEntity;
		}

		public static CrafterAchievementDto GetDomainDto(this CrafterAchievementDto achievementDto, CraftAchievement? persistedEntity = null)
		{
			if (persistedEntity is null)
			{
				persistedEntity = new CraftAchievement();
			}


			achievementDto.Description = persistedEntity.Description;
			achievementDto.CompletionDate = persistedEntity.CompletionDate;
			achievementDto.StartDate = persistedEntity.StartDate;
			achievementDto.IsPrivate = persistedEntity.IsPrivate;
			achievementDto.Title = persistedEntity.Title;
			achievementDto.RefIdentifier = persistedEntity.RefIdentifier;
			achievementDto.Slug = persistedEntity.Slug;
			achievementDto.Price = persistedEntity.Price;
			achievementDto.Currency = persistedEntity.Currency;
			achievementDto.MaterialsUsed = persistedEntity.MaterialsUsed;
			achievementDto.Status = persistedEntity.Status;
			achievementDto.Location = persistedEntity.Location;
			achievementDto.VideoUrl = persistedEntity.VideoUrl;


			return achievementDto;
		}

		public static AchievementImagesList GetList(this ICollection<AchievementImage> imagesList, IStoredImages imgService, ImgType imageType)
		{
			if (imagesList is null || imagesList.Count==0)
				return null;

			AchievementImagesList result = new();

			var imgs = imagesList.Select(achImg => new AchievementImageResponse()
			{
				Id = achImg.Id,
				ImageUrl = imgService.GetImage(achImg.ImgName, imageType).Result.FullPath ?? string.Empty,
				HeadingText = achImg.Heading
			}).ToList();

			result.AddRange(imgs);

			return result;
		}
	
	}
}
