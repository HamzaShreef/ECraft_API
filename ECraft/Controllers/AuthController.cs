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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mime;
using System.Security.Claims;
using System.Text.RegularExpressions;


namespace ECraft.Controllers
{
    [Produces(MediaTypeNames.Application.Json)]
	[Consumes(MediaTypeNames.Application.Json)]
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;
		private readonly IStoredImages _imgService;
		private readonly AppDbContext _db;
		ILogger<AuthController> _logger;

		public AuthController(IAuthService authService,AppDbContext db,ILogger<AuthController> logger,IStoredImages imgService)
		{
			_authService = authService;
			_db = db;
			_logger = logger;
			_imgService = imgService;
		}

		
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody]TokenRequest loginRequest)
		{
			if(ModelState.IsValid)
			{
				var authResult = await _authService.GetToken(loginRequest);

				return getResponseFromAuthResult(authResult);
				
			}
			else
			{
				return BadRequest(ModelState.GetErrorList());
			}
		}

		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody]AccountRequest registerRequest)
		{
			if (ModelState.IsValid)
			{
				var authResult = await _authService.CreateAccount(registerRequest);

				return getResponseFromAuthResult(authResult);			
			}
			else
			{
				return BadRequest(ModelState.GetErrorList());
			}
		}

		[HttpPatch("profile")]
		[Authorize]
		public async Task<IActionResult> EditProfile([FromBody]JsonPatchDocument<EditAccountRequest> patchDoc)
		{
			try
			{
				int uid = int.Parse(HttpContext.User.FindFirstValue("uid") ?? "0");
				var profile = await _db.Users.FindAsync(uid);

				if (profile == null)
					return NotFound();


				EditAccountRequest persitedInfo = new EditAccountRequest();
				persitedInfo = persitedInfo.GetDto(profile);


				patchDoc.ApplyTo(persitedInfo, ModelState);

				TryValidateModel(persitedInfo);

				if (!ModelState.IsValid)
					return BadRequest(ModelState.GetErrorList());

				ErrorList errors = new ErrorList();

				//UserName validation
				if (persitedInfo.UserName != profile.UserName)
				{
					if (string.IsNullOrEmpty(persitedInfo.UserName))
					{
						errors.AddError(AuthConstants.Errors.NullUserNameError, "Username cannot be left empty.");
						return BadRequest(errors);
					}
					bool reservedUserName = await _db.Users.AnyAsync(u => u.NormalizedUserName == persitedInfo.UserName.ToUpper());

					if (reservedUserName)
					{
						errors.AddError(AuthConstants.Errors.UsernameUsedError, "Use a different UserName");

						return BadRequest(errors);
					}
				}

				//CityId Validation
				if (persitedInfo.CityId != profile.CityId)
				{
					var existingCity = await _db.LCities.FirstOrDefaultAsync(c => c.Id == persitedInfo.CityId);

					if (existingCity is null)
					{
						errors.AddError(GeneralErrorCodes.InvalidCitySelection, "The CityId Parameter passed is incorrect");
						return NotFound(errors);
					}
					else
					{
						if (profile.CityId == null)
						{
							//concurrency issue passible here
							existingCity.UsersCount += 1;
						}
						profile.CityId = persitedInfo.CityId;
					}
				}



				//local means ther's no roundtrips to the database or anywhere else.
				bool localBusinessValidation;
				profile = persitedInfo.GetDomainEntity(out localBusinessValidation, out ErrorList? validationErrors, profile);

				if (!localBusinessValidation)
				{
					return BadRequest(validationErrors);
				}

				await _db.SaveChangesAsync();

				return Ok(persitedInfo);

			}
			catch(Exception ex) 
			{
				return this.ReturnServerDownError(ex, _logger);
			}

		}

		[HttpPut("profile/img")]
		[Authorize]
		[Consumes("multipart/form-data")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> UploadProfileImage([FromForm] UploadProfileImageDto profileImgDto)
		{
			var profileImg = profileImgDto.ImgFile;

			try
			{
				int uid = User.GetUserId();
				AppUser user = await _db.Users.FirstAsync(u => u.Id == uid);

				if(user.ProfileImg !=null)
				{
					var deletingResponse = await _imgService.RemoveImage(user.ProfileImg, ImgType.ProfileImage);
					if (!deletingResponse.Succeeded)
						return BadRequest(deletingResponse.Errors);
				}

				string fileName = $"USR-{uid}-{DateTime.UtcNow.ToString("yyyyMMddHHmmss")}";

				var validationResult = await _imgService.ValidateImage(profileImg);
				if (!validationResult.Succeeded)
				{
					return BadRequest(validationResult.Errors);
				}

				var storingResult = await _imgService.StoreImage(profileImg, ImgType.ProfileImage, fileName);
				user.ProfileImg = storingResult.ImgName;
				await _db.SaveChangesAsync();
				if (user == null)
				{
					return BadRequest();
				}

				return Ok(new { ImgUrl = storingResult.FullPath });

			}
			catch (Exception ex)
			{
				return this.ReturnServerDownError(ex, _logger);
			}
		}

		[HttpDelete("profile/img")]
		[Authorize]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> DeleteProfileImage()
		{
			try
			{
				int uid = User.GetUserId();
				AppUser user = await _db.Users.FirstAsync(u => u.Id == uid);

				if (user.ProfileImg != null)
				{
					var deletingResponse = await _imgService.RemoveImage(user.ProfileImg, ImgType.ProfileImage);
					if (!deletingResponse.Succeeded)
						return BadRequest(deletingResponse.Errors);

					user.ProfileImg = null;
					await _db.SaveChangesAsync();
					return Ok();
				}
				else
				{
					return NotFound();
				}

			}
			catch(Exception ex)
			{
				return this.ReturnServerDownError(ex, _logger);
			}
		}

		[HttpGet("profile")]
		[Authorize]
		public async Task<IActionResult> Profile()
		{
			int uid = User.GetUserId();

			var response = await _db.Users.Where(u => u.Id == uid).Select<AppUser, UserProfileResponse>(u => new UserProfileResponse()
			{
				Dob = u.Dob,
				UserId = u.Id,
				UserName = u.UserName,
				FirstName = u.FirstName,
				LastName = u.LastName,
				IsMale = u.MGender,
				Picture = u.ProfileImg,
				Email = u.Email ?? u.Id.ToString(),
			}).FirstOrDefaultAsync();

			if (response == null)
				return NotFound();

			return Ok(response);
		}


		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[HttpPost("token")]
		public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest refreshRequest)
		{
			if (ModelState.IsValid)
			{
				var authResult = await _authService.RefreshToken(refreshRequest);

				return getResponseFromAuthResult(authResult);
			}
			else
			{
				return BadRequest(ModelState.GetErrorList());
			}
		}


		//Private Methods
		private IActionResult getResponseFromAuthResult(AuthResult authResult)
		{
			if (authResult is null)
				throw new ArgumentNullException();

			if (authResult.Succeeded)
			{
				var responseDto = new AuthResponse()
				{
					AccessToken = authResult.AccessToken,
					Name = authResult.Name,
					Roles = authResult.Roles != null ? authResult.Roles.ToList() : new List<string>(),
					ImgUrl = authResult.ImgUrl,
					RefreshToken = authResult.RefreshToken,
					ExpiryDate = authResult.ExpriryDate,
					IsCrafter = authResult.IsCrafter,
				};

				return Ok(responseDto);
			}
			else
			{
				return BadRequest(authResult.Errors);
			}
		}
	}
}
