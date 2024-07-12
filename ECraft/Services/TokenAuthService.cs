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
using Microsoft.AspNetCore.DataProtection;
using ECraft.Constants;
using System.Text.Json;
using ECraft.Contracts.Response;
using System.Diagnostics;
using ECraft.Models.Identity;

namespace ECraft.Services
{
	public class TokenAuthService : IAuthService
	{
		private readonly IDataProtectionProvider _dataProtector;
		private readonly UserManager<AppUser> _userManager;
		private readonly AppDbContext _db;
		private readonly JwtSettings _jwtSettings;
		private readonly ILogger<TokenAuthService> _logger;
		private readonly TokenValidationParameters _tkValidationParameters;

		private ErrorList _tmpErrors;

        public TokenAuthService(IDataProtectionProvider dataProtector,UserManager<AppUser> userManager,AppDbContext dbContext,JwtSettings jwtSettings,TokenValidationParameters tokenValidationParameters,ILogger<TokenAuthService> tokenAuthServiceLogger)
        {
			_logger = tokenAuthServiceLogger;
			_dataProtector = dataProtector;
            _userManager = userManager;
			_jwtSettings = jwtSettings;
			_db = dbContext;
			_tmpErrors = new ErrorList();

			_tkValidationParameters = tokenValidationParameters;
        }


		public async Task<AuthResult> CreateAccount(AccountRequest registerRequest)
		{
			var newEndUser = new AppUser(registerRequest.FirstName,registerRequest.LastName);

			//UserName Validation
			var usedMail = await _userManager.Users.AnyAsync(u => u.NormalizedEmail == registerRequest.Email.ToUpper());

			
			var invalidResult = new AuthResult()
			{
				Succeeded = false,
				Errors = _tmpErrors
			};


			if (usedMail)
			{
				_tmpErrors.AddError(AuthConstants.Errors.EmailUsedError, "Email is already registered");
				return invalidResult;
			}

			newEndUser.Email = registerRequest.Email;
			newEndUser.UserName = registerRequest.Email;

			//Password Validation
			var validPassword = registerRequest.Password.Length >= 8;

			if (!validPassword)
			{
				_tmpErrors.AddError(AuthConstants.Errors.PasswordLengthError, "Password Length should be minimum of 8");
				return invalidResult;
			}

			//Date Of Birth Validation
			if(registerRequest.DateOfBirth!=null && registerRequest.DateOfBirth.HasValue)
			{
				bool validDOB = newEndUser.SetDob(registerRequest.DateOfBirth.Value);
				if (!validDOB)
				{
					_tmpErrors.AddError(AuthConstants.Errors.DobError, "Invalid Date Of Birth, Age does not meet the minimum requirement");
					//invalidResult.Errors = new List<string>() { "Invalid Date Of Birth, Age does not meet the minimum requirement" };
					return invalidResult;
				}
			}

			if (registerRequest.isMale.HasValue)
				newEndUser.MGender = registerRequest.isMale.Value;

			//Committing UserRecord
			var regResult = await _userManager.CreateAsync(newEndUser, registerRequest.Password);
			if (regResult.Succeeded)
			{
				return await generateAuthResultAsync(newEndUser);
			}
			else
			{
				invalidResult.Errors.AddRange(regResult.Errors);

				return invalidResult;
			}

		}

		public async Task<AuthResult> GetToken(TokenRequest tokenRequest)
		{
			_tmpErrors.AddError(AuthConstants.Errors.InvalidCredentialsError, "Invalid Credentials");

			var invalidResult = new AuthResult()
			{
				Errors = _tmpErrors
			};

			var userRecord = await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == tokenRequest.UserName.ToUpper());

			if (userRecord == null) 
			{
				return invalidResult;
			}

			var validCredentials = await _userManager.CheckPasswordAsync(userRecord, tokenRequest.Password);

			if(!validCredentials)
				return invalidResult;

			userRecord.LastLogin = DateTime.UtcNow;
			await _db.SaveChangesAsync();

			return await generateAuthResultAsync(userRecord);
		}

		public async Task<bool> IsTakenUserName(string userName)
		{
			//userName = userName.ToUpperInvariant();
			bool result = await _userManager.FindByNameAsync(userName) != null;
			return result;
		}

		public async Task<AuthResult> RefreshToken(RefreshTokenRequest refreshTokenRequest)
		{
			AuthResult authResult = new AuthResult()
			{
				Errors = _tmpErrors
			};

			var unprotectedPylodad = unprotectRefreshToken(refreshTokenRequest.RefreshToken);
			long? refreshId = unprotectedPylodad.Item2;
			string? jwtIdFromRefreshToken = unprotectedPylodad.Item1;


			if(refreshId ==null || jwtIdFromRefreshToken == null)
			{
				_tmpErrors.AddError(AuthConstants.Errors.RefreshTokenInvalidPayloadError, "Refresh Token Payload is Invalid");
				return authResult;
			}

			var tokenClaimsPrincipal = getPrincipalFromToken(refreshTokenRequest.AccessToken);
			
			
			//Validating the token
			if(tokenClaimsPrincipal == null)
			{
				_tmpErrors.AddError(AuthConstants.Errors.InvalidTokenError, "Invalid Token");
				return authResult;
			}

			string? jwtIdFromAccessToken = tokenClaimsPrincipal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

			if(jwtIdFromAccessToken == null)
			{
				_tmpErrors.AddError(AuthConstants.Errors.InvalidTokenError, "Token Payload has a missing field");
				return authResult;
			}


			//parsing token exp date
			var expiryDateUnix = long.Parse(tokenClaimsPrincipal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
			var expiryDateUtc=new DateTime(1970,1,1,0,0,0,DateTimeKind.Utc).AddSeconds(expiryDateUnix);

			if(expiryDateUtc  > DateTime.UtcNow && !_jwtSettings.RefreshInAccessPeriod)
			{
				_tmpErrors.AddError(AuthConstants.Errors.AccessNotExpiredError, "Access Token didn't Expire Yet");
				return authResult;
			}

			//checking the token existence
			var persistedRefreshToken = await _db.RefreshTokens.FindAsync(refreshId);

			if(persistedRefreshToken == null)
			{
				_tmpErrors.AddError(AuthConstants.Errors.NoPersistingRefreshTokenError, "This Refresh Token Does Not Exist");
				return authResult;
			}

			//Expiry Date Validation
			if(DateTime.UtcNow > persistedRefreshToken.ExpiryDate)
			{
				_tmpErrors.AddError(AuthConstants.Errors.ExpiredRefreshTokenError, "Refresh Token has expired user need to log in");
				return authResult;
			}

			if (persistedRefreshToken.IsInvalidated)
			{
				_tmpErrors.AddError(AuthConstants.Errors.RefreshTokenInvalidatedError, "Refresh Token Has been Invalidated For Security and the user need to log in");
				return authResult;
			}

			Claim? userIdClaim = tokenClaimsPrincipal.Claims.FirstOrDefault(c => c.Type == "uid");
			if (userIdClaim == null)
			{
				_tmpErrors.AddError(AuthConstants.Errors.InvalidTokenError, "Invalid Token Payload, Missing Claim");
				return authResult;
			}

			int uid = int.Parse(userIdClaim.Value);
			if (persistedRefreshToken.IsUsed)
			{
				_tmpErrors.AddError(AuthConstants.Errors.InvalidTokenError, "Malicious Action has been detected");
				_logger.LogWarning($"User with Id: {uid} is being attacked");
				//Maybe Lockout would be more secure

				try
				{
					var rowsAffectedCount = await _db.RefreshTokens
						.Where(rt => rt.UserId == uid && !rt.IsUsed)
						.ExecuteUpdateAsync(setters => setters.SetProperty(rfTk => rfTk.IsInvalidated, true));
					
				}
				catch (Exception ex)
				{
					//Logger Call
					_logger.LogCritical(ex,ex.Message);
					Debug.WriteLine(ex.Message);
				}

				return authResult;
			}

			if (!jwtIdFromAccessToken.Equals(jwtIdFromRefreshToken, StringComparison.InvariantCultureIgnoreCase))
			{
				_tmpErrors.AddError(AuthConstants.Errors.InvalidTokenError, "Refresh Token Does Not Match Access Token");
				return authResult;
			}

			persistedRefreshToken.IsUsed = true;
			_db.RefreshTokens.Update(persistedRefreshToken);
			await _db.SaveChangesAsync();

			AppUser? user = await _userManager.FindByIdAsync(userIdClaim.Value);
			
			if (user == null)
			{
				//Logger Call
				_tmpErrors.AddError(AuthConstants.Errors.InvalidTokenError, "Malicious Action Detected");
				return authResult;
			}

			return await generateAuthResultAsync(user);
		}

		public async Task<AuthResult> UpdateAccount(AppUser updatedUserRecord)
		{
			var updatingResult = await _userManager.UpdateAsync(updatedUserRecord);

			if (updatingResult.Succeeded)
				return new AuthResult() { Succeeded = true };
			else
			{
				var authResult = new AuthResult() { Succeeded = false, Errors = new ErrorList() };
				authResult.Errors.AddRange(updatingResult.Errors);
				return authResult;
			}
		}

		//Private Methods
		private async Task<AuthResult> generateAuthResultAsync(AppUser user)
		{
			var result = new AuthResult() { };

			if (user == null)
			{
				throw new NullReferenceException();
			}

			var tokenId = Guid.NewGuid().ToString().ToUpperInvariant();

			IEnumerable<Claim> tokenClaims = new List<Claim>()
			{
				new Claim(JwtRegisteredClaimNames.Sub,user.Email ?? user.Id.ToString()),
				new Claim(JwtRegisteredClaimNames.Email,user.Email),
				new Claim(JwtRegisteredClaimNames.Name,user.FirstName+" "+user.LastName),
				new Claim(JwtRegisteredClaimNames.Jti,tokenId),
				new Claim("uid",user.Id.ToString())
			};

			var userRoles = await _userManager.GetRolesAsync(user);
			var roleClaims = userRoles.Select(roleName => new Claim("role", roleName));

			tokenClaims = tokenClaims.Union(roleClaims);

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
			string jwtTokenString = jwtTokenHandler.WriteToken(jwtToken);

			result.Roles = userRoles;
			result.AccessToken = jwtTokenString;
			result.Succeeded = true;
			result.Name = user.FirstName + " " + user.LastName;
			result.ImgUrl = user.ProfileImg;
			result.IsCrafter = user.IsCrafter;
			result.ExpriryDate = DateTime.UtcNow + _jwtSettings.TokenLifeTime;

			
			result.RefreshToken = await generateRefreshToken(user, tokenId);

			return result;
		}

		private async Task<string> generateRefreshToken(AppUser user, string tokenId)
		{
			//generate a refresh token
			var refreshToken = new RefreshToken()
			{
				ExpiryDate = DateTime.UtcNow + _jwtSettings.RefreshPeriod,
				CreationDate = DateTime.UtcNow,
				JwtId = tokenId,
				IsUsed = false,
				UserId = user.Id,
			};

			_db.RefreshTokens.Add(refreshToken);
			await _db.SaveChangesAsync();

			long refreshTokenId = refreshToken.Id;
			string refreshTokenText = protectRefreshToken(tokenId, refreshTokenId);

			_db.RefreshTokens.Update(refreshToken);
			await _db.SaveChangesAsync();

			return refreshTokenText;
		}
		
		private string protectRefreshToken(string tokenId,long refreshTokenId)
		{
			var protector = _dataProtector.CreateProtector(AuthConstants.RefreshTokenCryptoPurpose);

			string payload = $"{tokenId}:{refreshTokenId}";

			var token = protector.Protect(payload);

			return token;
		}

		private (string?, long?) unprotectRefreshToken(string refreshToken)
		{
			var protector = _dataProtector.CreateProtector(AuthConstants.RefreshTokenCryptoPurpose);

			try
			{
				string res = protector.Unprotect(refreshToken);

				string[] payload = res.Split(":");
				string jwtId = payload[0];
				long refreshTokenId = long.Parse(payload[1]);

				return (jwtId, refreshTokenId);
			}
			catch(Exception ex)
			{
				//Logger Call
				Trace.WriteLine(ex.Message);
				return (null, null);
			}
		}

		private ClaimsPrincipal? getPrincipalFromToken(string token)
		{
			var tokenHandler = new JwtSecurityTokenHandler();

			try
			{
				_tkValidationParameters.ValidateLifetime = _jwtSettings.RefreshInAccessPeriod;
				var principal = tokenHandler.ValidateToken(token, _tkValidationParameters, out SecurityToken validatedToken);
				_tkValidationParameters.ValidateLifetime = _jwtSettings.ValidateLifeTime;

				if (validatedToken != null)
				{
					if (isJwtWithValidSecurityAlgorithm(validatedToken))
					{
						return principal;
					}

				}
			}
			catch
			{
				return null;
			}

			return null;
		}

		private bool isJwtWithValidSecurityAlgorithm(SecurityToken securityToken)
		{		
			if(securityToken is JwtSecurityToken jwtSecurityToken && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,StringComparison.InvariantCultureIgnoreCase))
			{
				return true;
			}

			return false;
		
		}

	}
}
