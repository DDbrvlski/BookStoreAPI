using System.ComponentModel.DataAnnotations;

namespace BookStoreViewModels.ViewModels.Accounts.User
{
    public class EmployeeUserPostViewModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }
        public string? PhoneNumber { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public string RoleName { get; set; }
    }
}
