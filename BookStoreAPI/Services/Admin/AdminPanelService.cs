using BookStoreAPI.Helpers;
using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreData.Data;
using BookStoreData.Models.Accounts;
using BookStoreViewModels.ViewModels.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BookStoreAPI.Services.Admin
{
    public interface IAdminPanelService
    {
        Task AddClaims(List<string> claimsToAdd);
        Task AddClaimsToRole(RoleClaimsPost roleClaims);
        Task AddNewRole(string roleName);
        Task RemoveClaims(string claimName);
        Task RemoveRole(string roleName);
        Task<IEnumerable<Claims>> GetAllClaimsAsync();
        Task<IEnumerable<ClaimValues>> GetAllClaimValuesAsync();
        Task<RoleClaimsPost> GetAllRoleClaimsAsync(string roleName);
        Task<List<string>> GetAllRolesAsync();
    }

    public class AdminPanelService
            (BookStoreContext context,
            RoleManager<IdentityRole> roleManager) : IAdminPanelService
    {
        public async Task AddNewRole(string roleName)
        {
            var roleExists = await roleManager.RoleExistsAsync(roleName);

            if (!roleExists)
            {
                var role = new IdentityRole(roleName);
                await roleManager.CreateAsync(role);
            }
            else
            {
                throw new BadRequestException("Podana nazwa roli już istnieje.");
            }
        }
        public async Task AddClaims(List<string> claimsToAdd)
        {
            List<Claims> claims = new List<Claims>();
            foreach (var claim in claimsToAdd)
            {
                claims.Add(new Claims()
                {
                    Name = claim
                });
            }

            await context.Claims.AddRangeAsync(claims);
        }
        public async Task AddClaimsToRole(RoleClaimsPost roleClaims)
        {
            var role = await roleManager.FindByNameAsync(roleClaims.RoleName);
            if (role == null)
            {
                throw new BadRequestException("Wystąpił błąd z pobieraniem roli.");
            }

            var existingRoleClaims = await roleManager.GetClaimsAsync(role);
            if (existingRoleClaims.Any())
            {
                List<Claim> newRoleClaims = new();

                foreach (var claim in roleClaims.ClaimPost)
                {
                    foreach (var claimValue in claim.ClaimValues)
                    {
                        newRoleClaims.Add(new Claim(claim.ClaimName, claimValue));
                    }
                }

                var claimsToRemove = existingRoleClaims.Where(existingClaim =>
                    !newRoleClaims.Any(newClaim =>
                        newClaim.Type == existingClaim.Type && newClaim.Value == existingClaim.Value
                    ))
                    .ToList();

                var claimsToAdd = newRoleClaims.Where(newClaim =>
                    !existingRoleClaims.Any(existingClaim =>
                        existingClaim.Type == newClaim.Type && existingClaim.Value == newClaim.Value
                    ))
                    .ToList();

                foreach (var claim in claimsToAdd)
                {
                    await roleManager.AddClaimAsync(role, claim);
                }
                foreach (var claim in claimsToRemove)
                {
                    await roleManager.RemoveClaimAsync(role, claim);
                }
            }
            else
            {
                foreach (var claim in roleClaims.ClaimPost)
                {
                    foreach (var claimValue in claim.ClaimValues)
                    {
                        await roleManager.AddClaimAsync(role, new Claim(claim.ClaimName, claimValue));
                    }
                }
            }
        }
        public async Task<List<string>> GetAllRolesAsync()
        {
            return await context.Roles.Select(x => x.Name).ToListAsync();
        }
        public async Task<RoleClaimsPost> GetAllRoleClaimsAsync(string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                throw new BadRequestException("Wystąpił błąd z pobieraniem roli.");
            }

            var existingRoleClaims = await roleManager.GetClaimsAsync(role);
            RoleClaimsPost roleClaims = new();
            roleClaims.RoleName = roleName;

            var groupedClaims = existingRoleClaims.GroupBy(c => c.Type)
                                      .ToDictionary(g => g.Key, g => g.Select(c => c.Value).ToList());

            roleClaims.ClaimPost = groupedClaims.Select(c => new ClaimsPost
            {
                ClaimName = c.Key,
                ClaimValues = c.Value
            }).ToList();

            return roleClaims;
        }
        public async Task<IEnumerable<Claims>> GetAllClaimsAsync()
        {
            return await context.Claims.Where(x => x.IsActive).ToListAsync();
        }
        public async Task<IEnumerable<ClaimValues>> GetAllClaimValuesAsync()
        {
            return await context.ClaimValues.Where(x => x.IsActive).ToListAsync();
        }
        public async Task RemoveClaims(string claimName)
        {
            var claim = await context.Claims.Where(x => x.IsActive && x.Name == claimName).FirstOrDefaultAsync();
            if (claim != null)
            {
                context.Claims.Remove(claim);
                await DatabaseOperationHandler.TryToSaveChangesAsync(context);
            }
        }
        public async Task RemoveRole(string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                throw new BadRequestException("Wystąpił błąd z pobieraniem roli.");
            }

            var roleClaims = await roleManager.GetClaimsAsync(role);
            foreach (var claim in roleClaims)
            {
                await roleManager.RemoveClaimAsync(role, claim);
            }

            var result = await roleManager.DeleteAsync(role);
            if (!result.Succeeded)
            {
                throw new Exception("Wystąpił błąd podczas usuwania roli.");
            }
        }
    }
}
