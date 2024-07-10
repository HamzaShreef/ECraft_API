using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECraft.Models.Identity
{
    [Owned]
	public class RefreshToken
	{
        [Key]
        public long Id { get; set; }

        public int UserId { get; set; }

        public bool IsInvalidated { get; set; }=false;

        public DateTime CreationDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        public AppUser User { get; set; }

        [MaxLength(100)]
        public string JwtId { get; set; }

        public bool IsUsed { get; set; } = false;
    }
}
