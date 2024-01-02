using BookStoreData.Models.Customers;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreData.Models.Accounts
{
    public class User : IdentityUser
    {
        public bool IsActive { get; set; } = true;
        #region Foreign Keys

        //Customer
        [Display(Name = "Dane klienta")]
        public int? CustomerID { get; set; }

        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; }

        #endregion
    }
}
