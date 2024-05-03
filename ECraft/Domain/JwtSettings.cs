namespace ECraft.Domain
{
	public class JwtSettings
	{
        public string Key { get; set; }

        public string Audience { get; set; }

        public string Issuer { get; set; }

        public bool ValidateLifeTime { get; set; }

        public TimeSpan TokenLifeTime { get; set; }

        public TimeSpan RefreshPeriod { get; set; }
    }
}
