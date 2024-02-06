namespace BookStoreViewModels.ViewModels.Accounts.User
{
    public class UserChangePasswordViewModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string RepeatNewPassword { get; set; }
    }
}
