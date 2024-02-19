using BookStoreData.Models.Helpers;
using System.ComponentModel.DataAnnotations;

namespace BookStoreData.Models.Notifications
{
    public class Newsletter : BaseEntity
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public DateTime PublicationDate { get; set; }
        public bool IsSent { get; set; } = false;
    }
}
