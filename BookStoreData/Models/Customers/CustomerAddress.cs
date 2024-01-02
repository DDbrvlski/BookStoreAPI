using BookStoreData.Models.Helpers;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookStoreData.Models.Customers
{
    public class CustomerAddress : BaseEntity
    {
        //Customer
        [Required(ErrorMessage = "Klient jest wymagany.")]
        [Display(Name = "Klient")]
        public int? CustomerID { get; set; }

        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; }

        //Address
        [Required(ErrorMessage = "Adres jest wymagany.")]
        [Display(Name = "Adres")]
        public int? AddressID { get; set; }

        [ForeignKey("AddressID")]
        public virtual Address Address { get; set; }
    }
}
