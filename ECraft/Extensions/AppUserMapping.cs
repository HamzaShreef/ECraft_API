using ECraft.Constants;
using ECraft.Contracts.Request;
using ECraft.Domain;
using ECraft.Models;
using Microsoft.AspNetCore.Identity;

namespace ECraft
{
	public static class AppUserMapping
	{
		public static AppUser GetDomainEntity(this EditProfileRequest user, out bool successfulMapping, out ErrorList? validationErrors, AppUser? oldInstance)
		{
			successfulMapping = true;
			validationErrors = null;

			if (oldInstance != null)
			{
				oldInstance.FirstName = user.FirstName;
				oldInstance.LastName = user.LastName;
				oldInstance.UserName = user.UserName;
				oldInstance.NormalizedUserName = user.UserName?.ToUpper();


				if (user.Dob != null && !oldInstance.SetDob(user.Dob.Value))
				{
					validationErrors = new ErrorList();
					validationErrors.AddError(AuthConstants.Errors.DobError, "Age doesn't meet the minimum requirements");

					successfulMapping = false;
				}


				return oldInstance;
			}
			else
			{
				var updatedInstance = new AppUser()
				{
					FirstName = user.FirstName,
					LastName = user.LastName,
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
