using BookStoreData.Models.Helpers;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BookStoreData.Models.Customers.AddressDictionaries;
using System.Text.Json.Serialization;

namespace BookStoreData.Models.Customers
{
    public class Address : BaseEntity
    {
        #region Properties
        public string? Street { get; set; }
        public string? StreetNumber { get; set; }
        public string? HouseNumber { get; set; }
        public string? Postcode { get; set; }
        public int? Position { get; set; } // 1 = address, 2 = mailing address
        #endregion
        #region Foreign Keys
        //City
        [Display(Name = "Miasto")]
        public int? CityID { get; set; }

        [ForeignKey("CityID")]
        [JsonIgnore]
        public virtual City? City { get; set; }

        //Country
        [Display(Name = "Kraj")]
        public int? CountryID { get; set; }

        [ForeignKey("CountryID")]
        [JsonIgnore]
        public virtual Country? Country { get; set; }
        #endregion
    }
}
