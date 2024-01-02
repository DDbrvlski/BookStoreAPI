using BookStoreData.Models.Helpers;

namespace BookStoreData.Models.Orders.Dictionaries
{
    public class DeliveryMethod : DictionaryTable
    {
        public decimal Price { get; set; }

        //1 = kurier, 2 = inpost
    }
}
