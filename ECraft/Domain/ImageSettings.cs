namespace ECraft.Domain
{
	public class ImageSettings
	{
        public string[] ValidExtensions { get; set; }

        public string HandiworkImage { get; set; }

        public int HandiworkMaxImagesCount { get; set; }

        public string ProfileImage { get; set; }

        public string OriginName { get; set; }

        public string RootFolder { get; set; }
    }
}
