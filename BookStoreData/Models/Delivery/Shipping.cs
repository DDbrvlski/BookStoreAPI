using BookStoreData.Models.Customers;
using BookStoreData.Models.Delivery.Dictionaries;
using BookStoreData.Models.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BookStoreData.Models.Delivery
{
    public class Shipping : BaseEntity
    {
        #region Properties
        public DateTime? ShippingDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        #endregion
        #region Foreign Keys

        //Address
        [Required(ErrorMessage = "Adres jest wymagany.")]
        [Display(Name = "Adres")]
        public int? AddressID { get; set; }

        [ForeignKey("AddressID")]
        [JsonIgnore]
        public virtual Address Address { get; set; }

        //ShippingStatus
        [Required(ErrorMessage = "Status wysyłki jest wymagany.")]
        [Display(Name = "Status wysyłki")]
        public int? ShippingStatusID { get; set; }

        [ForeignKey("ShippingStatusID")]
        [JsonIgnore]
        public virtual ShippingStatus ShippingStatus { get; set; }
        #endregion
    }
}
