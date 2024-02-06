using BookStoreData.Models.Helpers;
using System.Text.Json.Serialization;

namespace BookStoreData.Models.Customers
{
    public class Customer : BaseEntity
    {
        #region Properties
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsSubscribed { get; set; } = false;
        #endregion
        [JsonIgnore]
        public List<CustomerAddress>? CustomerAddresses { get; set; }
    }
}
