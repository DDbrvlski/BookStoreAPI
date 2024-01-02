using BookStoreData.Models.Helpers;
using BookStoreData.Models.Transactions.Dictionaries;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BookStoreData.Models.Transactions
{
    public class Payment : BaseEntity
    {
        #region Properties
        public decimal Amount { get; set; }
        public DateTime? Date { get; set; }
        #endregion
        #region Foreign Keys

        //PaymentMethod
        [Required(ErrorMessage = "Metoda płatności jest wymagana.")]
        [Display(Name = "Metoda płatności")]
        public int? PaymentMethodID { get; set; }

        [ForeignKey("PaymentMethodID")]
        [JsonIgnore]
        public virtual PaymentMethod PaymentMethod { get; set; }

        //TransactionStatus
        [Required(ErrorMessage = "Status transakcji jest wymagany.")]
        [Display(Name = "Status transakcji")]
        public int? TransactionStatusID { get; set; }

        [ForeignKey("TransactionStatusID")]
        [JsonIgnore]
        public virtual TransactionsStatus TransactionStatus { get; set; }
        #endregion
    }
}
