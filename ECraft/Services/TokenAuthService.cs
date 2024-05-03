using ECraft.Contracts.Request;
using ECraft.Data;
using ECraft.Domain;
using ECraft.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;

namespace ECraft.Services
{
	public class TokenAuthService : IAuthService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly AppDbContext _db;
		private readonly JwtSettings _jwtSettings;

        public TokenAuthService(UserManager<AppUser> userManager,AppDbContext dbContext,JwtSettings jwtSettings)
        {
            _userManager = userManager;
			_jwtSettings = jwtSettings;
			_db = dbContext;
        }


		public async Task<AuthResult> CreateAccount(AccountRequest registerRequest)
		{
			var newEndUser = new AppUser(registerRequest.FirstName,registerRequest.LastName);

			//UserName Validation
			var usedMail = await _userManager.Users.AnyAsync(u => u.NormalizedEmail == registerRequest.Email.ToUpper());

			var invalidResult = new AuthResult() { Succeeded = false, Errors = new List<string>() { "Email Is Used" } };

			if (usedMail)
			{
				return invalidResult;
			}

			newEndUser.Email = registerRequest.Email;
			newEndUser.UserName = registerRequest.Email;

			//Password Validation
			var validPassword = registerRequest.Password.Length >= 8;

			if (!validPassword)
			{
				invalidResult.Errors = new List<string>() { "Password Length should be minimum of 8" };
				return invalidResult;
			}

			//Date Of Birth Validation
			if(registerRequest.DateOfBirth!=null && registerRequest.DateOfBirth.HasValue)
			{
				bool validDOB = newEndUser.SetDob(registerRequest.DateOfBirth.Value);
				if (!validDOB)
				{
					invalidResult.Errors = new List<string>() { "Invalid Date Of Birth, Age does not meet the minimum requirement" };
					return invalidResult;
				}
			}

			if (registerRequest.isMale.HasValue)
				newEndUser.isMaleGender = registerRequest.isMale.Value;

			//Committing UserRecord
			var regResult = await _userManager.CreateAsync(newEndUser, registerRequest.Password);
			if (regResult.Succeeded)
			{
				return await generateAuthResultAsync(newEndUser);
			}
			else
			{
				invalidResult.Errors=regResult.Errors.Select<IdentityError,string>(err=>err.Description).ToList();
				return invalidResult;
			}

		}

		public async Task<AuthResult> GetToken(TokenRequest tokenRequest)
		{
			var invalidResult = new AuthResult()
			{
				Succeeded = false,
				Errors = new List<string>() { "Invalid Credentials" },
			};

			var userRecord = await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == tokenRequest.UserName.ToUpper());

			if (userRecord==null)
				return invalidResult;


			var validCredentials = await _userManager.CheckPasswordAsync(userRecord, tokenRequest.Password);

			if(!validCredentials)
				return invalidResult;

			return await generateAuthResultAsync(userRecord);
		}

		public async Task<bool> IsTakenUserName(string userName)
		{
			bool result = await _userManager.FindByNameAsync(userName) != null;
			return result;
		}

		public Task<AuthResult> RefreshToken(RefreshTokenRequest refreshTokenRequest)
		{
			throw new NotImplementedException();
		}

		public async Task<AuthResult> UpdateAccount(AppUser updatedUserRecord)
		{
			var updatingResult = await _userManager.UpdateAsync(updatedUserRecord);

			if (updatingResult.Succeeded)
				return new AuthResult() { Succeeded = true };
			else
				return new AuthResult() { Succeeded = false, Errors = updatingResult.Errors.Select(er => er.Description).ToList() };
		}

		private async Task<AuthResult> generateAuthResultAsync(AppUser user)
		{
			var result = new AuthResult() { };

			if(user==null)
			{
				throw new NullReferenceException();
			}

			IEnumerable<Claim> tokenClaims = new List<Claim>()
			{
				new Claim(JwtRegisteredClaimNames.Sub,user.Email ?? user.Id.ToString()),
				new Claim(JwtRegisteredClaimNames.Email,user.Email),
				new Claim(JwtRegisteredClaimNames.Name,user.FirstName+" "+user.LastName),
				new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
				new Claim("uid",user.Id.ToString())
			};

			var userRoles = await _userManager.GetRolesAsync(user);
			var roleClaims = userRoles.Select(roleName => new Claim("role", roleName));

			tokenClaims=tokenClaims.Union(roleClaims);

			var signatureKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
			var tokenDescriptor = new SecurityTokenDescriptor()
			{
				Expires = DateTime.UtcNow + _jwtSettings.TokenLifeTime,
				Audience = _jwtSettings.Audience,
				Issuer = _jwtSettings.Issuer,
				SigningCredentials = new SigningCredentials(signatureKey, SecurityAlgorithms.HmacSha256),
				Subject = new ClaimsIdentity(tokenClaims)
			};

			var jwtTokenHandler = new JwtSecurityTokenHandler();

			var jwtToken = jwtTokenHandler.CreateToken(tokenDescriptor);
			string jwtTokenString=jwtTokenHandler.WriteToken(jwtToken);

			result.Roles = userRoles;
			result.AccessToken = jwtTokenString;
			result.Succeeded = true;
			result.Name = user.FirstName +" "+ user.LastName;
			result.ImgUrl = user.ProfileImg;
			result.ExpriryDate = tokenDescriptor.Expires ?? DateTime.UtcNow+_jwtSettings.TokenLifeTime;

			return result;
		}

		private async Task<string> generateRefreshToken(AppUser user)
		{
			//Method Logic is Under Construction
			return await Task.FromResult(Guid.NewGuid().ToString());
		}
	}
}
