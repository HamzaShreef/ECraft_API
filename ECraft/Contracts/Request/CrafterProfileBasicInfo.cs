using ECraft.Constants;
using System.ComponentModel.DataAnnotations;

namespace ECraft.Contracts.Request
{
	public class CrafterProfileBasicInfo
	{
        public int CraftId { get; set; }

        [MinLength(10)]
        [MaxLength(12)]
        [DataType(DataType.PhoneNumber)]
        public string ContactPhone { get; set; }


        [MinLength(20)]
        [MaxLength(StringPropertyLengths.AboutCrafterMax)]
        public string? About { get; set; }

        [MinLength(3)]
        [MaxLength(StringPropertyLengths.CrafterTitleMax)]
        public string Title { get; set; }

		[MinLength(3)]
		[MaxLength(250)]
		public string? WorkLocation { get; set; }

        public double? LocationLatitude { get; set; }

        public double? LocationLongitude { get; set; }

    }
}
