using ECraft.Domain;

namespace ECraft.Services
{
	public interface IStoredImages
	{
		Task<ImageResult> StoreImage(IFormFile imgFile, ImgType imageType, string? recommendedUniqueName = null);

		Task<ImageResult> GetImage(string fileName, ImgType imgType);

		Task<ImageResult> ValidateImage(IFormFile imageFile);

		Task<ImageResult> RemoveImage(string fileName, ImgType imgType);

	}

	public enum ImgType
	{
		ProfileImage=1,
		HandiworkImage
	}

	public static class ImageExtensions
	{
		public static string GetDirectoryPath(this ImgType imageType, ImageSettings imageSettings)
		{
			string result = string.Empty;
			switch (imageType)
			{

				case ImgType.ProfileImage:
					result = imageSettings.ProfileImage;
					break;

				case ImgType.HandiworkImage:
					result = imageSettings.HandiworkImage;
					break;

			}
			return result;
		}
	}
}
