using System.ComponentModel.DataAnnotations;

namespace ECraft.Contracts.Request
{
	public class TokenRequest
	{
		[MinLength(10)]
		[MaxLength(256)]
        public string UserName { get; set; }

		[MinLength(8)]
		[MaxLength(30)]
		[DataType(DataType.Password)]
		public string Password { get; set; }
    }
}
