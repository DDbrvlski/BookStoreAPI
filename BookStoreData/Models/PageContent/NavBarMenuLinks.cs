using BookStoreData.Models.Helpers;

namespace BookStoreData.Models.PageContent
{
    public class NavBarMenuLinks : DictionaryTable
    {
        public string Path { get; set; }
        public int Position { get; set; }
    }
}
