using BookStoreData.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreData.Models.Notifications
{
    public class NewsletterSubscribers : BaseEntity
    {
        public string? Email { get; set; }        
    }
}
