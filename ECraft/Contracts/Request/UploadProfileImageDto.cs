using System.ComponentModel.DataAnnotations;

namespace ECraft.Contracts.Request
{
	public class UploadProfileImageDto
	{
		[Required]
		public IFormFile ImgFile { get; set; }
	}

}
