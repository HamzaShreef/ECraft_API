using ECraft.Constants;
using ECraft.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.Xml;

namespace ECraft.Contracts.Request
{
	public class EditAccountRequest
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

        public int? CityId { get; set; }

		[MaxLength(250)]
        public string? LocationText { get; set; }

    }
}
