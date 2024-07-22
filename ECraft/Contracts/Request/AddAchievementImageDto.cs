using ECraft.Constants;
using System.ComponentModel.DataAnnotations;

namespace ECraft.Contracts.Request
{
	public class AddAchievementImageDto
	{
        [Required]
        public IFormFile ImgFile { get; set; }

        [MaxLength(SizeConstants.AchievementImageHeadingMax)]
        public string? Text { get; set; }
    }
}
