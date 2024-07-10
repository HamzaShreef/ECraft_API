using ECraft.Contracts.Response;
using Microsoft.AspNetCore.Identity;

namespace ECraft.Domain
{
	public class AuthResult
	{
        public bool Succeeded { get; set; }=false;

        public ErrorList? Errors { get; set; }

		public IEnumerable<string>? Roles { get; set; }

        public string? ImgUrl { get; set; }

        public string Name { get; set; }

		public string AccessToken { get; set; }

		public string RefreshToken { get; set; }

        public DateTime ExpriryDate { get; set; }
    }
}
