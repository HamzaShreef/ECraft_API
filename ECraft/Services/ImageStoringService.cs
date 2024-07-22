using ECraft.Constants;
using ECraft.Domain;
using Microsoft.AspNetCore.StaticFiles;
using System.IO;
using System.Text.RegularExpressions;

namespace ECraft.Services
{
	public class ImageStoringService : IStoredImages
	{
		private readonly ImageSettings _pathConfig;

		public ImageStoringService(ImageSettings pathConfig)
		{
			_pathConfig = pathConfig;
		}

		public Task<ImageResult> ValidateImage(IFormFile imageFile)
		{
			ImageResult result = new();
			result.Errors = new ErrorList();

			// File Validation
			if (imageFile == null || imageFile.Length == 0 || !IsValidImage(imageFile, out _))
			{
				result.Errors.AddError(GeneralErrorCodes.InvalidImageFile, "Invalid Image File it must be [ Png, Gif, Jpeg, Jpg ]");

				return Task.FromResult(result);
			}

			if (imageFile.Length > 2 * 1024 * 1024)
			{
				result.Errors.AddError(GeneralErrorCodes.ImgFileMaxSize, "File Size should not exceed 2MB");
				return Task.FromResult(result);
			}

			// Image type validation can be added here if needed

			result.Succeeded = true;
			return Task.FromResult(result);
		}

		public Task<ImageResult> GetImage(string fileName, ImgType imageType)
		{
			if (fileName is null)
				return null;

			ImageResult result = new();
			result.Errors = new ErrorList();

			string subDirectory = imageType.GetDirectoryPath(_pathConfig);

			var rootFolder = _pathConfig.RootFolder + subDirectory;
			if (Directory.Exists(rootFolder))
			{
				var filePath = Path.Combine(rootFolder, fileName);

				if (File.Exists(filePath))
				{
					result.Succeeded = true;
					result.FullPath = _pathConfig.OriginName + subDirectory + fileName;
				}
			}
			else
			{
				result.Errors.AddError("Images_Directory_Notfound",$"{imageType} Images Directory Does Not Exist");
			}

			return Task.FromResult(result);
		}

		public Task<ImageResult> RemoveImage(string fileName, ImgType imageType)
		{
			ImageResult result = new();

			var folderPath = _pathConfig.RootFolder + imageType.GetDirectoryPath(_pathConfig);
			var filePath = Path.Combine(folderPath, fileName);

			if (File.Exists(filePath))
			{
				File.Delete(filePath);
				result.Succeeded = true;
			}
			else
			{
				result.Errors = new ErrorList();
				result.Errors.AddError("File_Not_Found", $"Image file does not exist: {fileName}");
			}

			return Task.FromResult(result);
		}

		public async Task<ImageResult> StoreImage(IFormFile imageFile,ImgType imageType,string? nameForPersisting=null)
		{
			ImageResult result = new();
			result.Errors = new ErrorList();

			string fileName = nameForPersisting ?? Guid.NewGuid().ToString();


			//File Validation
			if (imageFile == null || imageFile.Length == 0 || !IsValidImage(imageFile, out string fileExtension))
			{
				result.Errors.AddError(GeneralErrorCodes.InvalidImageFile, "Invalid Image File");
				return result;
			}

			fileName += fileExtension;

			if (imageFile?.Length > 2 * 1024 * 1024)
			{
				result.Errors.AddError(GeneralErrorCodes.ImgFileMaxSize, "File Size should not exceed 2MB");
				return result;
			}

			string subDirectory = imageType.GetDirectoryPath(_pathConfig);
			var rootFolder = _pathConfig.RootFolder + subDirectory;

			if (Directory.Exists(rootFolder))
			{
				var filePath = Path.Combine(rootFolder, fileName);

				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await imageFile.CopyToAsync(stream);
				}

				result.Succeeded = true;
				result.ImgName = fileName;
				result.FullPath = _pathConfig.OriginName + subDirectory + fileName;

				return result;
			}
			else
			{
				throw new DirectoryNotFoundException($"{imageType} Images Directory Does Not Exist");
			}

		}


		#region Private Methods

		private string GetContentType(string fileName)
		{

			var provider = new FileExtensionContentTypeProvider();
			if (!provider.TryGetContentType(fileName, out var contentType))
			{
				contentType = "application/octet-stream";
			}
			return contentType;

		}

		private bool IsValidImage(IFormFile file, out string fileExtension)
		{
			fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

			if (!_pathConfig.ValidExtensions.Contains(fileExtension))
			{
				return false;
			}

			using (var stream = file.OpenReadStream())
			{
				byte[] headerBytes = new byte[8];
				stream.Read(headerBytes, 0, headerBytes.Length);
				stream.Seek(0, SeekOrigin.Begin);

				if (IsJpeg(headerBytes) || IsPng(headerBytes) || IsGif(headerBytes))
				{
					return true;
				}
			}

			return false;
		}

		private static bool IsJpeg(byte[] headerBytes)
		{
			byte[] jpegMagicNumber = { 0xFF, 0xD8, 0xFF };
			byte[] jpegMagicNumberWithApp0 = { 0xFF, 0xD8, 0xFF, 0xE0 };
			byte[] jpegMagicNumberWithApp1 = { 0xFF, 0xD8, 0xFF, 0xE1 };

			if (headerBytes.Length >= jpegMagicNumberWithApp0.Length &&
				headerBytes.Take(jpegMagicNumberWithApp0.Length).SequenceEqual(jpegMagicNumberWithApp0))
			{
				return true;
			}

			if (headerBytes.Length >= jpegMagicNumberWithApp1.Length &&
				headerBytes.Take(jpegMagicNumberWithApp1.Length).SequenceEqual(jpegMagicNumberWithApp1))
			{
				return true;
			}

			// Check for the ".jpg" extension
			if (headerBytes.Length >= jpegMagicNumber.Length &&
				headerBytes.Take(jpegMagicNumber.Length).SequenceEqual(jpegMagicNumber))
			{
				return true;
			}

			return false;
		}


		private static bool IsPng(byte[] headerBytes)
		{
			byte[] pngMagicNumber = { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
			return headerBytes.Length >= pngMagicNumber.Length && headerBytes.Take(pngMagicNumber.Length).SequenceEqual(pngMagicNumber);
		}

		private static bool IsGif(byte[] headerBytes)
		{
			byte[] gifMagicNumber = { 0x47, 0x49, 0x46, 0x38 };
			return headerBytes.Length >= gifMagicNumber.Length && headerBytes.Take(gifMagicNumber.Length).SequenceEqual(gifMagicNumber);
		}

		#endregion

	}
}
