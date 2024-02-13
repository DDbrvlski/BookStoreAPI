using BookStoreData.Models.Helpers;

namespace BookStoreData.Models.Accounts
{
    public class ClaimValues : BaseEntity
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
