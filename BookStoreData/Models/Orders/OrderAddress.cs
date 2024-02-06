using BookStoreData.Models.Customers;
using BookStoreData.Models.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreData.Models.Orders
{
    public class OrderAddress : BaseEntity
    {
        //Order
        [Required(ErrorMessage = "Zamówienie jest wymagane.")]
        [Display(Name = "Zamówienie")]
        public int OrderID { get; set; }

        [ForeignKey("OrderID")]
        public virtual Order Order { get; set; }

        //Address
        [Required(ErrorMessage = "Adres jest wymagany.")]
        [Display(Name = "Adres")]
        public int AddressID { get; set; }

        [ForeignKey("AddressID")]
        public virtual Address Address { get; set; }
    }
}
