using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BookStoreData.Models.Helpers
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [JsonIgnore]
        public bool IsActive { get; set; } = true;

        [JsonIgnore]
        public DateTime CreationDate { get; set; } = DateTime.Now;
        [JsonIgnore]
        public DateTime? ModifiedDate { get; set; }
    }
}
