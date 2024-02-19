using System.ComponentModel.DataAnnotations;

namespace BookStoreDto.Dtos.Accounts.User
{
    public class UserDataDto
    {
        public string? UserId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Username { get; set; }

        public string? PhoneNumber { get; set; }
    }
}
