using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreDto.Dtos.Accounts.User
{
    public class UserPostDto
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
