using ECraft.Constants;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ECraft.Contracts.Request
{
    [Owned]
	public class CrafterProfileBasicInfo
	{
        public int CraftId { get; set; }

        [MinLength(10)]
        [MaxLength(12)]
        [DataType(DataType.PhoneNumber)]
        public string ContactPhone { get; set; }


        [MinLength(20)]
        [MaxLength(SizeConstants.AboutCrafterMax)]
        public string? About { get; set; }

        [MinLength(3)]
        [MaxLength(SizeConstants.CrafterTitleMax)]
        public string Title { get; set; }

		[MinLength(3)]
		[MaxLength(250)]
		public string? WorkLocation { get; set; }

        public double? LocationLatitude { get; set; }

        public double? LocationLongitude { get; set; }

    }
}
