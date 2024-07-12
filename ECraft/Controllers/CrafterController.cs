using ECraft.Constants;
using ECraft.Contracts.Request;
using ECraft.Contracts.Response;
using ECraft.Data;
using ECraft.Domain;
using ECraft.Extensions;
using ECraft.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ECraft.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	//[Authorize(Roles ="Crafter")]
	public class CrafterController : ControllerBase
	{
		private readonly AppDbContext _db;
		private readonly ILogger<CrafterController> _logger;

		public CrafterController(AppDbContext db, ILogger<CrafterController> logger)
		{
			_db = db;
			_logger = logger;
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
					return NotFound("Referenced Craft Not Found");

				//Hit 3
				LocationCity? requesterCity = await _db.LCities.FindAsync(crafterInfo.CityId);


				//CityId Validation
				if (requesterCity is null)
				{
					return NotFound("Referenced City Not Found");
				}


				userRecord.CityId = crafterInfo.CityId;

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
				requesterCity.CraftersCount += 1;
				craft.CraftersCount += 1;
				await _db.SaveChangesAsync(); //Hit 5

				userRecord.IsCrafter = true;
				userRecord.CrafterProfileId = newCrafter.Id;
				//Hit 4
				await _db.SaveChangesAsync();

				return Ok(crafterInfo);
			}
			else
				return BadRequest(ModelState.GetErrorList());

		}


		[HttpPatch]
		[Authorize(Roles ="Crafter")]
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

				crafterProfile.CraftId = persitedInfo.CraftId;
				crafterProfile.ContactPhone = persitedInfo.ContactPhone;
				crafterProfile.Title = persitedInfo.Title;
				crafterProfile.About = persitedInfo.About;
				crafterProfile.WorkLocation = persitedInfo.WorkLocation;

				await _db.SaveChangesAsync();

				return Ok(persitedInfo);


			}
			catch(Exception ex)
			{
				_logger.LogCritical(ex,ex.Message);
				return StatusCode(StatusCodes.Status500InternalServerError);
				//Logger Call
			}
		}


		[HttpGet]
		[Authorize(Roles = "Crafter")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> GetProfile()
		{
			int uid = User.GetUserId();

			var profile = await _db.Crafters.FirstOrDefaultAsync(c => c.UserId == uid);
			if (profile is null)
				return NotFound();

			CrafterProfileResponse response = new CrafterProfileResponse();
			response = response.GetResponseDto(profile);

			
				



			return Ok(response);

		}
		
		//AddSkill
		//GetProfile
		//GetRequesterProfile [Authorize]
		//AddReview
		//UnReview
		//Like
		//Unlike
		//LikeReview
	
	}
}
