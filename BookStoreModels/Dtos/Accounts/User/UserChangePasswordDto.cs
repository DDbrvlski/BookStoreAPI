using System.ComponentModel.DataAnnotations;

namespace BookStoreDto.Dtos.Accounts.User
{
    public class UserChangePasswordDto
    {
        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string RepeatNewPassword { get; set; }
    }
}
