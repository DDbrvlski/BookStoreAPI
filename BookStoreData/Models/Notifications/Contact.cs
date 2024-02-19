using BookStoreData.Models.Helpers;
using System.ComponentModel.DataAnnotations;

namespace BookStoreData.Models.Notifications
{
    public class Contact : BaseEntity
    {
        [Required]
        public string ClientName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Content { get; set; }
    }
}
