using System.ComponentModel.DataAnnotations;

namespace BookStoreDto.Dtos.Claims
{
    public class RoleClaimsPostDto
    {
        [Required]
        public string RoleName { get; set; }
        [Required]
        public List<ClaimsPostDto> ClaimPost { get; set; }
    }
}
