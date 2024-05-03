using ECraft.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.Xml;

namespace ECraft.Contracts.Request
{
	public class EditProfileRequest:IDtoMapping<EditProfileRequest,AppUser>
	{
		[MaxLength(20)]
		[MinLength(3)]
        public string FirstName { get; set; }

		[MaxLength(20)]
		[MinLength(3)]
        public string LastName { get; set; }

		[DataType(DataType.Date)]
        public DateOnly? Dob { get; set; }

		[RegularExpression(@"^[a-z]{3,10}[_]{0,1}[a-z]{0,10}[0-9]{0,4}$", ErrorMessage = "Invalid UserName Format")]
		public string? UserName { get; set; }



		public AppUser GetDomainEntity(out bool successfulMapping, out string validationErrorMessage, AppUser? oldInstance)
		{
			successfulMapping = true;
			validationErrorMessage = string.Empty;

			if (oldInstance != null)
			{
				oldInstance.FirstName = this.FirstName;
				oldInstance.LastName = this.LastName;
				oldInstance.UserName = this.UserName;
				oldInstance.NormalizedUserName = this.UserName?.ToUpper();

				validationErrorMessage = string.Empty;

				if (this.Dob != null && !oldInstance.SetDob(this.Dob.Value))
				{
					validationErrorMessage = "Age doesn't meet the minimum requirements";
					successfulMapping = false;
				}


				return oldInstance;
			}
			else
			{
				var updatedInstance = new AppUser(this.FirstName, this.LastName)
				{
					UserName = this.UserName
				};

				return updatedInstance;
			}
		}

		public EditProfileRequest GetDto(AppUser domainEntity)
		{
			if (domainEntity is null)
				return null;

			this.FirstName = domainEntity.FirstName;
			this.LastName = domainEntity.LastName;
			this.UserName = domainEntity.UserName;
			this.Dob = domainEntity.Dob;

			return this;
		}
	}
}
