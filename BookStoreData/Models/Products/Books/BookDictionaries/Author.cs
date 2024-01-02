using BookStoreData.Models.Helpers;

namespace BookStoreData.Models.Products.Books.BookDictionaries
{
    public class Author : DictionaryTable
    {
        public string Surname { get; set; }
        public string Description { get; set; }
    }
}
