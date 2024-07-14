using ECraft.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ECraft.Contracts.LocationDto
{
	public class CityDto
	{
		public int CityId { get; set; }

		[MinLength(2)]
		[MaxLength(100)]
		public string CityName { get; set; }

		public int CountryId { get; set; }

		public int? RegionId { get; set; }

	}
}
