namespace ECraft.Contracts.Response
{
	public class AuthResponse
	{
        public string Name { get; set; }

        public string AccessToken { get; set; }

        public List<string>? Roles { get; set; }

        public string? ImgUrl { get; set; }

        public string RefreshToken { get; set; }

        public DateTime ExpiryDate { get; set; }
    }
}
