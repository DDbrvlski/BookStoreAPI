using BookStoreData.Data;
using BookStoreViewModels.ViewModels.Claims;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Services.Policies
{
    public class PolicyService(BookStoreContext context)
    {
        public async Task<List<PolicyClaims>> CreateAuthorizationPoliciesAsync()
        {
            var policyClaims = new List<PolicyClaims>();
            var claims = await context.Claims.ToListAsync();
            var claimValues = await context.ClaimValues.ToListAsync();

            foreach (var claim in claims)
            {
                foreach (var claimValue in claimValues)
                {
                    policyClaims.Add(new PolicyClaims()
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
