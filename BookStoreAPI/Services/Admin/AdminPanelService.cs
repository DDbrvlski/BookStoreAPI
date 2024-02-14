using BookStoreAPI.Helpers;
using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreAPI.Services.Customers;
using BookStoreAPI.Services.Users;
using BookStoreData.Data;
using BookStoreData.Models.Accounts;
using BookStoreViewModels.ViewModels.Accounts.User;
using BookStoreViewModels.ViewModels.Admin;
using BookStoreViewModels.ViewModels.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;

namespace BookStoreAPI.Services.Admin
{
    public interface IAdminPanelService
    {
        Task AddClaims(List<string> claimsToAdd);
        Task AddClaimsToRole(RoleClaimsPost roleClaims);
        Task AddNewRole(string roleName);
        Task DeactivateUserAsync(string userId);
        Task EditEmployeeDataAsync(EmployeeDataEditViewModel employeeDetails);
        Task<IEnumerable<Claims>> GetAllClaimsAsync();
        Task<IEnumerable<ClaimValues>> GetAllClaimValuesAsync();
        Task<RoleClaimsPost> GetAllRoleClaimsAsync(string roleName);
        Task<List<string>> GetAllRolesAsync();
        Task<EmployeeDetailsViewModel> GetEmployeeDetailsAsync(string userId);
        Task<IEnumerable<EmployeeDataViewModel>> GetEmployeesAsync();
        Task RemoveClaims(string claimName);
        Task RemoveRole(string roleName);
    }

    public class AdminPanelService
            (BookStoreContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager,
            IUserService userService)
            : IAdminPanelService
    {
        public async Task<IEnumerable<EmployeeDataViewModel>> GetEmployeesAsync()
        {
            var roles = await roleManager.Roles.Where(x => x.Name != "User").ToListAsync();
            List<EmployeeDataViewModel> employees = new();
            foreach (var role in roles)
            {
                var usersInRole = await userManager.GetUsersInRoleAsync(role.Name);
                foreach (var user in usersInRole)
                {
                    if (user.IsActive)
                    {
                        employees.Add(new EmployeeDataViewModel
                        {
                            Id = user.Id,
                            Username = user.UserName,
                            Email = user.Email,
                            PhoneNumber = user.PhoneNumber,
                            RoleName = role.Name,
                        });
                    }
                }
            }

            return employees;
        }
        public async Task<EmployeeDetailsViewModel> GetEmployeeDetailsAsync(string userId)
        {
            var user = await context.User
                .Where(x => x.IsActive && x.Id == userId)
                .Select(x => new EmployeeDetailsViewModel()
                {
                    Id = x.Id,
                    Name = x.Customer.Name,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    Surname = x.Customer.Surname,
                    Username = x.UserName,
                })
                .FirstAsync();

            var userRoles = await GetUserRolesAsync(userId);
            user.RoleNames = userRoles.ToList();

            return user;
        }
        public async Task EditEmployeeDataAsync(EmployeeDataEditViewModel employeeDetails)
        {
            var user = await context.User.Where(x => x.IsActive && x.Id == employeeDetails.Id).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new BadRequestException("Wystąpił błąd podczas pobierania usera.");
            }
            await userService.EditUserDataAsync(new UserDataViewModel().CopyProperties(employeeDetails));

            if (employeeDetails.Password != null)
            {
                var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);
                await userManager.ResetPasswordAsync(user, resetToken, employeeDetails.Password);
            }

            var userRoles = await userManager.GetRolesAsync(user);

            var rolesToAdd = employeeDetails.RoleNames.Where(x => !userRoles.Contains(x)).ToList();
            var rolesToRemove = userRoles.Except(employeeDetails.RoleNames).ToList();

            if (rolesToAdd.Any())
            {
                await userManager.AddToRolesAsync(user, rolesToAdd);
            }
            if (rolesToRemove.Any())
            {
                await userManager.RemoveFromRolesAsync(user, rolesToRemove);
            }
        }
        public async Task DeactivateUserAsync(string userId)
        {
            var user = await context.User.Where(x => x.IsActive && x.Id == userId).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new BadRequestException("Wystąpił błąd podczas pobierania usera.");
            }

            user.IsActive = false;
            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }
        private async Task<IEnumerable<string>> GetUserRolesAsync(string userId)
        {
            var user = await context.User.Where(x => x.IsActive && x.Id == userId).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new BadRequestException("Wystąpił błąd podczas pobierania usera.");
            }

            return await userManager.GetRolesAsync(user);
        }
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
            var existingClaims = await context.Claims.Where(x => x.IsActive && claimsToAdd.Any(y => x.Name == y)).Select(x => x.Name).ToListAsync();
            claimsToAdd = claimsToAdd.Where(x => !existingClaims.Contains(x)).ToList();

            if (claimsToAdd.Any())
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
                await DatabaseOperationHandler.TryToSaveChangesAsync(context);
            }
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
