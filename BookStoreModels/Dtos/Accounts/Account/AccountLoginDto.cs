using System.ComponentModel.DataAnnotations;

namespace BookStoreDto.Dtos.Accounts.Account
{
    public class AccountLoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Audience { get; set; }
    }
}
