using BookStoreData.Models.Helpers;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BookStoreData.Models.Customers
{
    public class CustomerHistory : BaseEntity
    {
        #region Properties
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        #endregion
        //Customer
        [Required(ErrorMessage = "Klient jest wymagany.")]
        [Display(Name = "Klient")]
        public int? CustomerID { get; set; }

        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; }

        [JsonIgnore]
        public List<CustomerAddress>? CustomerAddresses { get; set; }
    }
}
