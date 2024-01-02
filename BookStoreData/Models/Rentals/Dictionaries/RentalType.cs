using BookStoreData.Models.Helpers;

namespace BookStoreData.Models.Rentals.Dictionaries
{
    public class RentalType : DictionaryTable
    {
        public int Days { get; set; }
        public decimal Price { get; set; }
    }
}
