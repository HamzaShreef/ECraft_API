namespace ECraft.Domain
{
	public class ImageResult
	{
		public bool Succeeded { get; set; } = false;

        public ErrorList? Errors { get; set; }

        public string ImgName { get; set; }

        public string? FullPath { get; set; }
    }
}
