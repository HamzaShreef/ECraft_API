using ECraft.Contracts.Request;
using ECraft.Domain;
using ECraft.Models;
using System.Security.Claims;

namespace ECraft.Services
{
	public interface IAuthService
	{
		Task<AuthResult> GetToken(TokenRequest tokenRequest);

		Task<AuthResult> CreateAccount(AccountRequest registerRequest);

		Task<AuthResult> UpdateAccount(AppUser updatedUserRecord);

		Task<AuthResult> RefreshToken(RefreshTokenRequest refreshTokenRequest);

		Task<bool> IsTakenUserName(string userName);
    }
}
