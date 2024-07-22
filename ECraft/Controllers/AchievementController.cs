using ECraft.Constants;
using ECraft.Contracts.Request;
using ECraft.Contracts.Response;
using ECraft.Data;
using ECraft.Domain;
using ECraft.Extensions;
using ECraft.Models;
using ECraft.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECraft.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	//[Authorize(Roles ="Crafter")]
	public class AchievementController : ControllerBase
	{
		private readonly AppDbContext _appDbContext;
		private readonly ILogger<AchievementController> _logger;
		private readonly IStoredImages _imgService;

        public AchievementController(AppDbContext appDbContext,ILogger<AchievementController> logger,IStoredImages imgService)
        {
			_logger = logger;
            _appDbContext = appDbContext;
			_imgService = imgService;
        }

        [HttpPost]
		[Authorize(Roles ="Crafter")]
		public async Task<IActionResult> AddAchievement([FromBody] CrafterAchievementDto achievementInfo,CancellationToken ct)
		{
			if (ModelState.IsValid)
			{
				try
				{
					int crafterProfileId = User.GetCrafterProfileId();


					var crafterProfile = await _appDbContext.Crafters.FirstAsync(c => c.Id == crafterProfileId, ct);

					var newCraftAchievement = new CraftAchievement()
					{
						CrafterId = crafterProfileId,
						PublishDate = DateTime.UtcNow,
						CraftId=crafterProfile.CraftId
					};

					newCraftAchievement = achievementInfo.GetDomainEntity(newCraftAchievement);

					_appDbContext.Add(newCraftAchievement);
					crafterProfile.AchievementsCount++;
					await _appDbContext.SaveChangesAsync(ct);

					var response = new AchievementResponse();
					response.AchievementId = newCraftAchievement.Id;
					response.MutableInfo = achievementInfo;
					response.PublishDate = newCraftAchievement.PublishDate;

					return Ok(response);
				}
				catch (Exception ex)
				{
					return this.ReturnServerDownError(ex, _logger);
				}

			}else
				return BadRequest(ModelState.GetErrorList());
		}

		[HttpGet("All")]
		//[Authorize(Roles = "Crafter")]
		public async Task<IActionResult> GetCrafterAchievements([FromQuery] long profileId, [FromQuery] PaginationFilter requestFilter, CancellationToken ct)
		{
			if (ModelState.IsValid)
			{
				try
				{
					IQueryable<CraftAchievement> query = _appDbContext.CraftAchievements
						.Where(a => a.CrafterId == profileId && !a.IsPrivate);

					if (User.IsInRole("Crafter"))
					{

						int requesterCrafterId = User.GetCrafterProfileId();


						//Customizing response for requester
						if (requesterCrafterId == profileId)
						{
							query = _appDbContext.CraftAchievements
								.Where(a => a.CrafterId == profileId);
						}
					}


					requestFilter.PageNumber = Math.Max(requestFilter.PageNumber, 1);
					requestFilter.PageSize = Math.Min(requestFilter.PageSize, SizeConstants.GenericPageSize);

					var queryResult = await query.Include(a => a.Images)
						.OrderByDescending(a => a.LikesCount)
						.ThenByDescending(a => a.ViewCount)
						.Skip(requestFilter.PageSize * (requestFilter.PageNumber - 1))
						.Take(requestFilter.PageSize)
						.Select(ach => new AchievementResponse()
						{
							MutableInfo = new CrafterAchievementDto().GetDomainDto(ach),
							PublishDate = ach.PublishDate,
							AchievementId = ach.Id,
							IntroImgUrl = ach.IntroImg == null ? null : _imgService.GetImage(ach.IntroImg, ImgType.HandiworkImage).Result.FullPath,
							ImagesList = ach.Images == null ? null : ach.Images.GetList(_imgService, ImgType.HandiworkImage)

						})
						.ToListAsync(ct);

					PagedResponse<AchievementResponse> response = new PagedResponse<AchievementResponse>(queryResult)
					{
						PageSize = requestFilter.PageSize,
						PageNumber = requestFilter.PageNumber
					};

					return Ok(response);
				}
				catch (Exception ex)
				{
					return this.ReturnServerDownError(ex, _logger);
				}

			}
			else
				return BadRequest(ModelState.GetErrorList());

		}


		[HttpPost("img/{Id}")]
		[Authorize(Roles ="Crafter")]
		public async Task<IActionResult> AddAchievementImage(long Id, [FromForm] AddAchievementImageDto imgDto)
		{
			if (ModelState.IsValid)
			{
				int crafterId = User.GetCrafterProfileId();
				var achievement = await _appDbContext.CraftAchievements.FindAsync(Id);

				if (achievement is null)
					return NotFound();

				if (crafterId != achievement.CrafterId)
				{
					return Forbid();
				}

				var validatingResult = await _imgService.ValidateImage(imgDto.ImgFile);
				if (!validatingResult.Succeeded)
				{
					return BadRequest(validatingResult.Errors);
				}



				string imgName = $"ACH-{achievement.Id}-{DateTime.UtcNow.ToString("yyyyMMddHHmmss")}";
				var storingResult = await _imgService.StoreImage(imgDto.ImgFile, ImgType.HandiworkImage,imgName);
				if (storingResult.Succeeded)
				{
					AchievementImageResponse response = new AchievementImageResponse()
					{
						Id = 0,
						ImageUrl = storingResult.FullPath ?? string.Empty,
						HeadingText=imgDto.Text
					};

					if (achievement.IntroImg is null)
					{
						achievement.IntroImg = storingResult.ImgName;
						await _appDbContext.SaveChangesAsync();

						return Ok(response);
					}
					else
					{
						var newAchievementImage = new AchievementImage()
						{
							ImgName = storingResult.ImgName,
							Heading = imgDto.Text,
							AchievementId = achievement.Id,
							UploadDate = DateTime.UtcNow			
						};
						_appDbContext.Add(newAchievementImage);
						await _appDbContext.SaveChangesAsync();

						response.Id = newAchievementImage.Id;
						return Ok(response);
					}
				}
				else
				{
					return BadRequest(storingResult.Errors);
				}

			}
			else
			{
				return BadRequest(ModelState.GetErrorList());
			}
		}

		[HttpPatch]
		[Authorize(Roles = "Crafter")]
		public async Task<IActionResult> EditAchievement([FromBody] JsonPatchDocument<CrafterAchievementDto> patchDoc)
		{
			return Ok("Under Construction");
		}
	}
}
