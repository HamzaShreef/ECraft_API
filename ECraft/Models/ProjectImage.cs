using System.ComponentModel.DataAnnotations;

namespace ECraft.Models
{
	public class ProjectImage:BaseEntity<int>
	{
        public int ProjectId { get; set; }

        public CraftProject? Project { get; set; }

        [MaxLength(100)]
		public string ImgName { get; set; }

		[MaxLength(250)]
        public string? Heading { get; set; }

        public int LikesCount { get; set; }
    }
}
