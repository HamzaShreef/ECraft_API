namespace ECraft.Contracts.Response
{
	public class PrivateProfileResponse
	{
        public PublicProfileResponse PublicInfo { get; set; }

        public int ViewsCount { get; set; }

    }
}
