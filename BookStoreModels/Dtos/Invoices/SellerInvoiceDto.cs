using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreDto.Dtos.Invoices
{
    public class SellerInvoiceDto
    {
        public string Name { get; set; }
        public string TaxIdentificationNumber { get; set; } //NIP
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public string? HouseNumber { get; set; }
        public string Postcode { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
