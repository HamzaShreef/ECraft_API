namespace ECraft.Domain
{
	public class AuthResult
	{
        public bool Succeeded { get; set; }

        public IEnumerable<string>? Errors { get; set; }

		public IEnumerable<string>? Roles { get; set; }

        public string? ImgUrl { get; set; }

        public string Name { get; set; }

		public string AccessToken { get; set; }

		public string RefreshToken { get; set; }

        public DateTime ExpriryDate { get; set; }
    }
}
