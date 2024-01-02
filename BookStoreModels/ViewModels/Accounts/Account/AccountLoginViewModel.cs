using System.ComponentModel.DataAnnotations;

namespace BookStoreViewModels.ViewModels.Accounts.Account
{
    public class AccountLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public string? Audience { get; set; }
    }
}
