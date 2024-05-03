namespace ECraft.Contracts.Response
{
	public class UserProfileResponse
	{
        public int UserId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public bool isMale { get; set; }

        public DateOnly? Dob { get; set; }

        public string? UserName { get; set; }

        public string? Picture { get; set; }

    }
}
