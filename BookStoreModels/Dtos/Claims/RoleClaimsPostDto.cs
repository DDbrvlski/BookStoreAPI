namespace BookStoreDto.Dtos.Claims
{
    public class RoleClaimsPostDto
    {
        public string RoleName { get; set; }
        public List<ClaimsPostDto> ClaimPost { get; set; }
    }
}
