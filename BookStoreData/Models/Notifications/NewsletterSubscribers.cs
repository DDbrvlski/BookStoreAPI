using BookStoreData.Models.Helpers;

namespace BookStoreData.Models.Notifications
{
    public class NewsletterSubscribers : BaseEntity
    {
        public string Email { get; set; }        
    }
}
