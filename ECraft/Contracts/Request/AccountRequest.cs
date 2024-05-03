using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ECraft.Contracts.Request
{
	public class AccountRequest
	{

        [MaxLength(20)]
        [MinLength(2)]
        public string FirstName { get; set; }

        [MaxLength(20)]
        [MinLength(2)]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        public DateOnly? DateOfBirth { get; set; }

		public bool? isMale { get; set; }

        [DataType(DataType.EmailAddress)]
        [MaxLength(256)]
        public string Email { get; set; }

        [MaxLength(30)]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
