﻿using BookStoreData.Models.Customers;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BookStoreData.Models.Accounts
{
    public class User : IdentityUser
    {
        public bool IsActive { get; set; } = true;
        #region Foreign Keys

        //Customer
        [Required]
        [Display(Name = "Dane klienta")]
        public int CustomerID { get; set; }

        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; }

        #endregion
    }
}
