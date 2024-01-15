using System.ComponentModel.DataAnnotations;

namespace BookStoreViewModels.ViewModels.Customers
{
    public class CustomerGuestViewModel
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
    }
}
