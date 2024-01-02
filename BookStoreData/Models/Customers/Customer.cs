using BookStoreData.Models.Helpers;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BookStoreData.Models.Orders;

namespace BookStoreData.Models.Customers
{
    public class Customer : BaseEntity
    {
        #region Properties
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public bool IsSubscribed { get; set; }
        #endregion
        [JsonIgnore]
        public List<CustomerAddress>? CustomerAddresses { get; set; }
    }
}
