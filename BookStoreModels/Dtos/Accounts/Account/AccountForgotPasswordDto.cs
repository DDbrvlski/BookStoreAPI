using System.ComponentModel.DataAnnotations;

namespace BookStoreDto.Dtos.Accounts.Account
{
    public class AccountForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
