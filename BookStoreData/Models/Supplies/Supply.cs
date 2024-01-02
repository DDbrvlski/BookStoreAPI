using BookStoreData.Models.Helpers;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BookStoreData.Models.Supplies.Dictionaries;
using BookStoreData.Models.Transactions.Dictionaries;
using System.Text.Json.Serialization;

namespace BookStoreData.Models.Supply
{
    public class Supply : BaseEntity
    {
        #region Properties
        public DateTime DeliveryDate { get; set; }
        #endregion
        #region Foreign Keys
        //Supplier
        [Required(ErrorMessage = "Dostawca jest wymagana.")]
        [Display(Name = "Dostawca")]
        public int? SupplierID { get; set; }

        [ForeignKey("SupplierID")]
        [JsonIgnore]
        public virtual Supplier Supplier { get; set; }

        //DeliveryStatus
        [Required(ErrorMessage = "Status dostawy jest wymagany.")]
        [Display(Name = "Status dostawy")]
        public int? DeliveryStatusID { get; set; }

        [ForeignKey("DeliveryStatusID")]
        [JsonIgnore]
        public virtual DeliveryStatus DeliveryStatus { get; set; }

        //PaymentMethod
        [Required(ErrorMessage = "Metoda płatności jest wymagana.")]
        [Display(Name = "Metoda płatności")]
        public int? PaymentMethodID { get; set; }

        [ForeignKey("PaymentMethodID")]
        [JsonIgnore]
        public virtual PaymentMethod PaymentMethod { get; set; }
        #endregion
    }
}
