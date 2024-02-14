using BookStoreData.Data;
using BookStoreDto.Dtos.Claims;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Services.Policies
{
    public class PolicyService(BookStoreContext context)
    {
        public async Task<List<PolicyClaimsDto>> CreateAuthorizationPoliciesAsync()
        {
            var policyClaims = new List<PolicyClaimsDto>();
            var claims = await context.Claims.ToListAsync();
            var claimValues = await context.ClaimValues.ToListAsync();

            foreach (var claim in claims)
            {
                foreach (var claimValue in claimValues)
                {
                    policyClaims.Add(new PolicyClaimsDto()
                    {
                        PolicyName = $"{claim.Name}{claimValue.Name}",
                        ClaimName = claim.Name,
                        ClaimValue = claimValue.Value
                    });
                }
            }

            return policyClaims;
        }
    }
}
