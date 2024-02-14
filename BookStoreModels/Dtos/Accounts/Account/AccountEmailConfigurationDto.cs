namespace BookStoreDto.Dtos.Accounts.Account
{
    public class AccountEmailConfigurationDto
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? SmtpServer { get; set; }
        public int Port { get; set; }
        public bool EnableSSL { get; set; }
    }
}
