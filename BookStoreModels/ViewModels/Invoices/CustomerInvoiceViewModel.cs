namespace BookStoreViewModels.ViewModels.Invoices
{
    public class CustomerInvoiceViewModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public CustomerAddressInvoiceViewModel Address { get; set; }
    }
}
