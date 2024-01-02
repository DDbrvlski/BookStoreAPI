using BookStoreData.Models.Helpers;

namespace BookStoreData.Models.Media
{
    public class Images : BaseEntity
    {
        public string? Title { get; set; }
        public string? ImageURL { get; set; }
        public int? Position { get; set; } = 1;
    }
}
