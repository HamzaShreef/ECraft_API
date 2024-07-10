using ECraft.Constants;
using ECraft.Contracts.Request;
using ECraft.Contracts.Response;
using ECraft.Data;
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
		private readonly AppDbContext _db;

		public AuthController(IAuthService authService,AppDbContext db)
		{
			_authService = authService;
			_db = db;
		}

		
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody]TokenRequest loginRequest)
		{
			if(ModelState.IsValid)
			{
				var authResult = await _authService.GetToken(loginRequest);


				if(authResult.Succeeded)
				{
					var responseDto = new AuthResponse()
					{
						AccessToken = authResult.AccessToken,
						Name = authResult.Name,
						Roles = authResult.Roles != null ? authResult.Roles.ToList() : new List<string>(),
						ImgUrl = authResult.ImgUrl,
						RefreshToken = authResult.RefreshToken,
						ExpiryDate=authResult.ExpriryDate
					};

					return Ok(responseDto);
				}
				else
				{
					return BadRequest(authResult.Errors);
				}
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


				if (authResult.Succeeded)
				{
					var responseDto = new AuthResponse()
					{
						AccessToken = authResult.AccessToken,
						Name = authResult.Name,
						Roles = authResult.Roles != null ? authResult.Roles.ToList() : new List<string>(),
						ImgUrl = authResult.ImgUrl,
						RefreshToken = authResult.RefreshToken,
					};

					return Ok(responseDto);
				}
				else
				{
					return BadRequest(authResult.Errors);
				}
			}
			else
			{
				return BadRequest(ModelState.GetErrorList());
			}
		}

		[HttpPatch("profile")]
		[Authorize]
		public async Task<IActionResult> EditProfile([FromBody]JsonPatchDocument<EditProfileRequest> patchDoc)
		{
			int uid = int.Parse(HttpContext.User.FindFirstValue("uid") ?? "0");
			var profile = await _db.Users.FindAsync(uid);

			if (profile == null)
				return NotFound();


			EditProfileRequest persitedInfo = new EditProfileRequest();
			persitedInfo = persitedInfo.GetDto(profile);
			

			patchDoc.ApplyTo(persitedInfo,ModelState);

			TryValidateModel(persitedInfo);

			if (!ModelState.IsValid)
				return BadRequest(ModelState.GetErrorList());

			List<IdentityError> errors = new List<IdentityError>();

			//UserName validation
			if (persitedInfo.UserName != profile.UserName)
			{
				if (string.IsNullOrEmpty(persitedInfo.UserName))
				{
					var error = new IdentityError() { Code = AuthConstants.Errors.NullUserNameError, Description = "Username cannot be left empty." };
					errors.Add(error);
					return BadRequest(errors);
				}
				bool reservedUserName = await _db.Users.AnyAsync(u => u.NormalizedUserName == persitedInfo.UserName.ToUpper());

				if (reservedUserName)
				{
					var error = new IdentityError() { Code = AuthConstants.Errors.UsernameUsedError, Description = "Use a different UserName" };
					errors.Add(error);
					return BadRequest(errors);
				}
			}



			//local means ther's no roundtrips to the database or anywhere else.
			bool localBusinessValidation;
			profile = persitedInfo.GetDomainEntity(out localBusinessValidation, out IdentityError validationError, profile);

			if (!localBusinessValidation)
			{
				errors.Add(validationError);
				return BadRequest(errors);
			}


			var updateResult = await _authService.UpdateAccount(profile);
			if (updateResult.Succeeded)
				return Ok(persitedInfo);
			else
				return StatusCode(StatusCodes.Status500InternalServerError,updateResult.Errors);


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
				Name = u.FirstName + " " + u.LastName,
				isMale=u.MGender,
				Picture=u.ProfileImg,
				Email=u.Email ?? u.Id.ToString(),
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


				if (authResult.Succeeded)
				{
					var responseDto = new AuthResponse()
					{
						AccessToken = authResult.AccessToken,
						Name = authResult.Name,
						Roles = authResult.Roles != null ? authResult.Roles.ToList() : new List<string>(),
						ImgUrl = authResult.ImgUrl,
						RefreshToken = authResult.RefreshToken,
						ExpiryDate = authResult.ExpriryDate
					};

					return Ok(responseDto);
				}
				else
				{
					return BadRequest(authResult.Errors);
				}
			}
			else
			{
				return BadRequest(ModelState.GetErrorList());
			}
		}
	}
}
