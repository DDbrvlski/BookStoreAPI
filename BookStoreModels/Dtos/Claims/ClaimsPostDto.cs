using System.ComponentModel.DataAnnotations;

namespace BookStoreDto.Dtos.Claims
{
    public class ClaimsPostDto
    {
        [Required]
        public string ClaimName { get; set; }
        [Required]
        public List<string> ClaimValues { get; set; }
    }
}
