using ECraft.Contracts.Request;
using ECraft.Models;

namespace ECraft
{
	public static class AppUserMapping
	{
		public static AppUser GetDomainEntity(this EditProfileRequest user, out bool successfulMapping, out string validationErrorMessage, AppUser? oldInstance)
		{
			successfulMapping = true;
			validationErrorMessage = string.Empty;

			if (oldInstance != null)
			{
				oldInstance.FirstName = user.FirstName;
				oldInstance.LastName = user.LastName;
				oldInstance.UserName = user.UserName;
				oldInstance.NormalizedUserName = user.UserName?.ToUpper();

				validationErrorMessage = string.Empty;

				if (user.Dob != null && !oldInstance.SetDob(user.Dob.Value))
				{
					validationErrorMessage = "Age doesn't meet the minimum requirements";
					successfulMapping = false;
				}


				return oldInstance;
			}
			else
			{
				var updatedInstance = new AppUser(user.FirstName, user.LastName)
				{
					UserName = user.UserName
				};

				return updatedInstance;
			}
		}

		public static EditProfileRequest GetDto(this EditProfileRequest user, AppUser domainEntity)
		{
			if (domainEntity is null)
				return null;

			user.FirstName = domainEntity.FirstName;
			user.LastName = domainEntity.LastName;
			user.UserName = domainEntity.UserName;
			user.Dob = domainEntity.Dob;

			return user;
		}
	}
}
