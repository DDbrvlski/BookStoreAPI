namespace BookStoreDto.Dtos.Accounts.User
{
    public class UserChangePasswordDto
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string RepeatNewPassword { get; set; }
    }
}
