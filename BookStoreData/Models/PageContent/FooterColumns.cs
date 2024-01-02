using BookStoreData.Models.Helpers;

namespace BookStoreData.Models.PageContent
{
    public class FooterColumns : DictionaryTable
    {
        public int Position { get; set; }
        public string HTMLObject { get; set; }
        public string Direction { get; set; }
    }
}
