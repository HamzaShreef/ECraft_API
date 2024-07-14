namespace ECraft.Contracts.Request
{
	public class CrafterReviewDto
	{
        public int ProfileId { get; set; }

        public string Comment { get; set; }

		public byte StarCount { get; set; } = 0;

		public int? ProjectId { get; set; }
	}
}
