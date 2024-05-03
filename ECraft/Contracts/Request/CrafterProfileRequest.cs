using System.ComponentModel.DataAnnotations;

namespace ECraft.Contracts.Request
{
	public class CrafterProfileRequest
	{
        public int CraftId { get; set; }

        [MaxLength(20)]
        [MinLength(10)]
        [DataType(DataType.PhoneNumber)]
        public string ContactPhone { get; set; }

        [Required]
        public int CityId { get; set; }

        [MinLength(20)]
        [MaxLength(500)]
        public string About { get; set; }
    }
}
