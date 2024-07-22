using ECraft.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ECraft.Contracts.LocationDto
{
	public class CountryDto
	{
		public int CountryId { get; set; }

		[MinLength(2)]
		[MaxLength(50)]
		public string CountryName { get; set; }

		[MaxLength(5)]
		[MinLength(2)]
		public string CountryCode { get; set; }

		[MinLength(2)]
		[MaxLength(50)]
        public string? LocalName { get; set; }

    }
}
