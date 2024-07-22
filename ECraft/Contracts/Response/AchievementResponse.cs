using ECraft.Contracts.Request;

namespace ECraft.Contracts.Response
{
	public class AchievementResponse
	{
        public CrafterAchievementDto MutableInfo { get; set; }

        public long AchievementId { get; set; }

        public DateTime PublishDate { get; set; }

        public string? IntroImgUrl { get; set; }

        public AchievementImagesList? ImagesList { get; set; }
    }

    public class AchievementImageResponse
    {
        public long Id { get; set; }

        public string ImageUrl { get; set; }

        public string? HeadingText { get; set; }
    }

    public class AchievementImagesList : List<AchievementImageResponse>
    {
        public void AddImage(int Id, string ImageUrl,string headingText = null)
        {
            AchievementImageResponse img = new AchievementImageResponse()
            {
                Id = Id,
                ImageUrl = ImageUrl,
                HeadingText = headingText
            };

            Add(img);
        }
    }
}
