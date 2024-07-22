using ECraft.Constants;
using ECraft.Contracts.Request;
using ECraft.Contracts.Response;
using ECraft.Data;
using ECraft.Domain;
using ECraft.Extensions;
using ECraft.Models;
using ECraft.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Reflection.Metadata;

namespace ECraft.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	//[Authorize(Roles ="Crafter")]
	public class CrafterController : ControllerBase
	{
		private readonly AppDbContext _db;
		private readonly ILogger<CrafterController> _logger;
		private readonly IStoredImages _imgService;

		public CrafterController(AppDbContext db, ILogger<CrafterController> logger,IStoredImages imgService)
		{
			_db = db;
			_logger = logger;
			_imgService = imgService;
		}

		[HttpPost]
		[Authorize]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> CreateAccount([FromBody] CrafterProfileBasicInfo crafterInfo)
		{
			ErrorList errors = new ErrorList();


			if (ModelState.IsValid)
			{
				try
				{
					int uid = User.GetUserId();
					//Hit 1
					AppUser? userRecord = await _db.Users.FindAsync(uid);

					if (userRecord is null)
					{
						//Malicious Action
						//Logger Call
						_logger.LogCritical("Malicious Action: User Record deleted while they try to join crafters");
						return NotFound();
					}

					//CrafterProfile? existingProfile = await _db.Crafters.FirstOrDefaultAsync(c => c.UserId == uid);
					if (userRecord.CrafterProfileId is not null)
					{
						errors.AddError(GeneralErrorCodes.CrafterProfileAlreadyCreated, "There is already a crafter profile created for this user");
						return BadRequest(errors);
					}

					//Hit 2
					Craft? craft = await _db.Crafts.FindAsync(crafterInfo.CraftId);

					if (craft is null)
					{
						errors.AddError(GeneralErrorCodes.InvalidCraftSelection, "Referenced Craft Not Found");
						return NotFound(errors);
					}


					if (userRecord.CityId is null)
					{
						errors.AddError(GeneralErrorCodes.CitySelectionRequired, "User should have an assigned city on their account");
						return BadRequest(errors);
					}


					//Hit 3
					LocationCity? requesterCity = await _db.LCities.FindAsync(userRecord.CityId);


					CrafterProfile newCrafter = new CrafterProfile()
					{
						UserId = uid,
						JoinDate = DateTime.UtcNow
					};

					newCrafter = crafterInfo.GetDomainEntity(newCrafter);



					_db.Crafters.Add(newCrafter);
					_db.UserRoles.Add(new Models.Identity.AppUserRole()
					{
						UserId = uid,
						RoleId = AuthConstants.CrafterRoleId
					});
					requesterCity.CraftersCount++;
					craft.CraftersCount++;
					await _db.SaveChangesAsync(); //Hit 4

					//await _db.Users.Where(u => u.Id == uid)
					//	.ExecuteUpdateAsync(u => u.SetProperty(usr => usr.IsCrafter, true)
					//							  .SetProperty(usr => usr.CrafterProfileId, newCrafter.Id));

					userRecord.IsCrafter = true;
					userRecord.CrafterProfileId = newCrafter.Id;
					//Hit 5
					await _db.SaveChangesAsync();

					return Ok(crafterInfo);
				}
				catch (Exception ex)
				{
					return this.ReturnServerDownError(ex, _logger);
				}
			}
			else
				return BadRequest(ModelState.GetErrorList());

		}


		[HttpPatch]
		[Authorize(Roles = "Crafter")]
		public async Task<IActionResult> EditAccount([FromBody] JsonPatchDocument<CrafterProfileBasicInfo> crafterPatchDoc)
		{
			ErrorList errors = new ErrorList();

			try
			{
				int uid = User.GetUserId();


				CrafterProfile? crafterProfile = await _db.Crafters.FirstOrDefaultAsync(c => c.UserId == uid);


				if (crafterProfile is null)
				{
					_logger.Log(LogLevel.Critical, "User who is not Crafter is accessing EditCrafterProfile Endpoint");
					return NotFound();
				}

				CrafterProfileBasicInfo persitedInfo = new CrafterProfileBasicInfo();
				persitedInfo = persitedInfo.GetDto(crafterProfile);


				crafterPatchDoc.ApplyTo(persitedInfo, ModelState);

				TryValidateModel(persitedInfo);

				if (!ModelState.IsValid)
					return BadRequest(ModelState.GetErrorList());

				crafterProfile = persitedInfo.GetDomainEntity(crafterProfile);

				await _db.SaveChangesAsync();

				return Ok(persitedInfo);


			}
			catch (Exception ex)
			{
				return this.ReturnServerDownError(ex, _logger);
			}
		}


		[HttpGet]
		[Authorize(Roles = "Crafter")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> GetRequesterProfile(CancellationToken ct)
		{
			int uid = User.GetUserId();

			return await getProfile(c => c.UserId == uid, ct, true);


			//try
			//{

			//	var profile = await _db.Database.SqlQueryRaw<PrivateProfileResponse>($"Exec GetCrafterProfile {uid}").FirstOrDefaultAsync(ct);
			//	if (profile is null)
			//		return NotFound();
			//	else
			//		return Ok(profile);
			//}
			//catch (Exception ex)
			//{
			//	return this.ReturnServerDownError(ex, _logger);
			//}


		}


		[HttpGet("{Id}")]
		[Authorize]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetProfile(int Id, CancellationToken ct)
		{
			IActionResult profileResponse = await getProfile(c => c.Id == Id, ct);

			return profileResponse;
		}


		[HttpPut("Review")]
		[Authorize]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> AddReview([FromBody] CrafterReviewDto crafterReview,CancellationToken ct)
		{
			int uid = User.GetUserId();

			try
			{
				bool profileExists = await _db.Crafters.AnyAsync(c => c.Id == crafterReview.ProfileId, ct);
				if (!profileExists)
					return NotFound();

				var existingReview = await _db.CraftersReviews
					.FirstOrDefaultAsync(crv => crv.ReviewerId == uid 
										&& crv.ProfileId == crafterReview.ProfileId
										&& crv.AchievementId == null);

				if (existingReview is null)
				{

					var newReview = new CrafterReview()
					{
						ReviewerId = uid,
						ProfileId = crafterReview.ProfileId,
						AchievementId = null,
						StarCount = crafterReview.StarCount,
						Comment = crafterReview.Comment,
						ReviewDate = DateTime.UtcNow
					};

					_db.CraftersReviews.Add(newReview);

					await incrementReviewsCount(crafterReview.ProfileId);
				}
				else
				{
					existingReview.StarCount = crafterReview.StarCount;
					existingReview.Comment = crafterReview.Comment;
					existingReview.ReviewDate = DateTime.UtcNow;
				}

				await _db.SaveChangesAsync(ct);

				return Ok(crafterReview);
			}
			catch(Exception ex)
			{
				return this.ReturnServerDownError(ex, _logger);
			}
			

		}

		[HttpDelete("Review")]
		[Authorize]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> UnReview([FromQuery]long revId, CancellationToken ct)
		{
			int uid = User.GetUserId();

			try
			{
				var crafterReview = await _db.CraftersReviews.FirstOrDefaultAsync(rev => rev.Id == revId && rev.ReviewerId == uid, ct);
				if (crafterReview is null)
				{
					return NotFound();
				}
			

				_db.CraftersReviews.Remove(crafterReview);
				await _db.SaveChangesAsync(ct);

				if (crafterReview.AchievementId is null)
					await incrementReviewsCount(crafterReview.ProfileId ?? 0, increment: false);

				return Ok();
			}
			catch (Exception ex)
			{
				return this.ReturnServerDownError(ex, _logger);
			}


		}

		[HttpPost("Interaction")]
		[Authorize]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> AddLike([FromQuery] int profileId, [FromQuery] int interactionType, CancellationToken ct)
		{
			int uid = User.GetUserId();

			try
			{
				bool profileExists = await _db.Crafters.AnyAsync(c => c.Id == profileId, ct);
				if (!profileExists)
					return NotFound();

				UserInteraction? userInteraction = await _db.UserInteractions
								.FirstOrDefaultAsync(interaction => interaction.InteractorId == uid
													  && interaction.ProfileId == profileId
													  && interaction.AchievementId == null
													  && interaction.ImgId == null
													  , ct);


				if (userInteraction is not null)
				{
					userInteraction.InteractionType = Math.Min(interactionType, 5);
					userInteraction.IDate = DateTime.UtcNow;
				}
				else
				{
					var newInteraction = new UserInteraction()
					{
						InteractorId = uid,
						ProfileId = profileId,
						IDate = DateTime.UtcNow,
						InteractionType = Math.Min(interactionType, 5)
					};

					_db.UserInteractions.Add(newInteraction);

					//Incrementing a counter
					int rowsAffected = await incrementLikesCount(profileId);
				}

				await _db.SaveChangesAsync(ct);

				return Ok();
			}
			catch (Exception ex)
			{
				return this.ReturnServerDownError(ex, _logger);
			}


		}

		[HttpDelete("Interaction")]
		[Authorize]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> UnLike([FromQuery] int profileId, CancellationToken ct)
		{
			int uid = User.GetUserId();

			try
			{
				UserInteraction? userInteraction = await _db.UserInteractions
								.FirstOrDefaultAsync(interaction => interaction.InteractorId == uid
													  && interaction.ProfileId == profileId
													  && interaction.AchievementId == null
													  && interaction.ImgId == null
													  , ct);

				if (userInteraction is null)
					return NotFound();


				_db.UserInteractions.Remove(userInteraction);
				await _db.SaveChangesAsync(ct);


				int rowsAffected = await incrementLikesCount(profileId, increment: false);


				return Ok();
			}
			catch (Exception ex)
			{
				return this.ReturnServerDownError(ex, _logger);
			}


		}




		//LikeReview
		//AddSkill

		//Private Methods
		private async Task<IActionResult> getProfile(Expression<Func<CrafterProfile, bool>> predicate, CancellationToken ct, bool includePrivate = false)
		{
			try
			{
				int uid = User.GetUserId();

				var crafterProfile = await _db.Crafters.Where(predicate)
					.Select(c => new
					{
						Profile = c,
						UserProfile = new UserProfileResponse
						{
							UserId = c.UserRecord.Id,
							FirstName = c.UserRecord.FirstName,
							LastName = c.UserRecord.LastName,
							UserName = c.UserRecord.UserName,
							Picture = c.UserRecord.ProfileImg == null ? null : _imgService.GetImage(c.UserRecord.ProfileImg,ImgType.ProfileImage).Result.FullPath, 
							IsMale = c.UserRecord.MGender
						},
						CrafterCraft = c.Craft,
						CrafterCity = c.UserRecord.City,
						CrafterCountry = c.UserRecord.City != null ? c.UserRecord.City.Country : null,
						CrafterRegion = c.UserRecord.City != null ? c.UserRecord.City.Region : null
					}).FirstOrDefaultAsync(ct);


                CrafterProfile? profile = crafterProfile?.Profile;


				if (crafterProfile is null || profile is null)
				{
					return NotFound();
				}

				PublicProfileResponse response = new PublicProfileResponse();
				response = response.GetResponseDto(profile, crafterProfile.CrafterCraft, crafterProfile.UserProfile, crafterProfile.CrafterCity, crafterProfile.CrafterCountry, crafterProfile.CrafterRegion);

				


				//If it's not the requester profile
				if (!includePrivate)
				{
				
					var spRowsAffected = await _db.Database
						.ExecuteSqlRawAsync($"Exec ViewCrafterProfile @ProfileId={profile.Id},@ViewerId={uid};", ct);

					return Ok(response);
				}
				else
				{
					var privateResponse=new PrivateProfileResponse();
					privateResponse = privateResponse.GetPrivateInfo(response, profile);
					return Ok(privateResponse);
				}
			}
			catch (Exception ex)
			{
				return this.ReturnServerDownError(ex, _logger, "Something went Wrong While Fetching data, sorry for that we'll fix it soon");
			}

		}
		

		private async Task<int> incrementLikesCount(int profileId,bool increment = true)
		{
			if (increment)
			{
				var res = await _db.Database
					.ExecuteSqlAsync($"Update dbo.Crafters Set LikesCount=LikesCount+1 Where Id = {profileId};");

				return res;
			}
			else
			{
				var res = await _db.Database
					.ExecuteSqlAsync($"Update dbo.Crafters Set LikesCount=LikesCount-1 Where Id = {profileId};");

				return res;
			}
		}
		

		private async Task<int> incrementReviewsCount(int profileId, bool increment = true)
		{
			if (increment)
			{
				var res = await _db.Database
					.ExecuteSqlAsync($"Update dbo.Crafters Set ReviewsCount=ReviewsCount+1 Where Id = {profileId};");

				return res;
			}
			else
			{
				var res = await _db.Database
					.ExecuteSqlAsync($"Update dbo.Crafters Set ReviewsCount=ReviewsCount-1 Where Id = {profileId};");

				return res;
			}
		}
	}
}
