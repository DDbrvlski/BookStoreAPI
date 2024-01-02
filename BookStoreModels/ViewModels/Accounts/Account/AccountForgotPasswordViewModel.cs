using System.ComponentModel.DataAnnotations;

namespace BookStoreViewModels.ViewModels.Accounts.Account
{
    public class AccountForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
