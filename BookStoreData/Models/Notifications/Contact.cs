using BookStoreData.Models.Helpers;
using System.ComponentModel.DataAnnotations;

namespace BookStoreData.Models.Notifications
{
    public class Contact : BaseEntity
    {
        public string ClientName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Content { get; set; }
    }
}
