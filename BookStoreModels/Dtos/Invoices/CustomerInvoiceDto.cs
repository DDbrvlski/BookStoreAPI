namespace BookStoreDto.Dtos.Invoices
{
    public class CustomerInvoiceDto
    {
        public string Name { get; set; } = "";
        public string Surname { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public CustomerAddressInvoiceDto Address { get; set; }
    }
}
