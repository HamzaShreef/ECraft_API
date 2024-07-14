using ECraft.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ECraft.Contracts.LocationDto
{
	public class StateDto
	{
        public int RegionId { get; set; }

        [MaxLength(100)]
		[MinLength(2)]
        public string RegionName { get; set; }

        public int CountryId { get; set; }
	}
}
