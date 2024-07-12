namespace ECraft.Constants
{
	public static class AuthConstants
	{
		public const string RefreshTokenCryptoPurpose = "RefreshToken";

		public const int CrafterRoleId = 2;

        public static class Errors
		{
			public const string EmailUsedError = "MailTaken";

			public const string PasswordLengthError = "PasswordMinLength8";

			public const string DobError = "MinAge18";

			public const string InvalidCredentialsError = "IncorrectCreds";

			public const string NullUserNameError = "NullUsername";

			public const string UsernameUsedError = "UsernameTaken";

			public const string InvalidTokenError = "InvalidToken";

			public const string RefreshTokenInvalidPayloadError = "RefreshInvalidPayload";

			public const string AccessNotExpiredError = "AccessTokenNotExpired";

			public const string NoPersistingRefreshTokenError = "RefreshTokenNotPersisting";

			public const string ExpiredRefreshTokenError = "RefreshExpired";

			public const string RefreshTokenInvalidatedError = "RefreshInvalidated";

			public const string RefreshTokenUsedError = "MaliciousAction";

		}
    }
}
