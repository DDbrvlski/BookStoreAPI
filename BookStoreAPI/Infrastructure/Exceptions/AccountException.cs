namespace BookStoreAPI.Infrastructure.Exceptions
{
    public class AccountException : Exception
    {
        public List<string> ErrorMessages { get; }

        public AccountException(string message) : base(message)
        {

        }
        public AccountException(List<string> errorMessages)
        {
            ErrorMessages = errorMessages;
        }
    }
}
