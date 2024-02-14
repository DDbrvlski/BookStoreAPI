using BookStoreAPI.Services.Users;
using BookStoreData.Models.Accounts;
using BookStoreDto.Dtos.Admin;
using Microsoft.AspNetCore.Identity;

namespace BookStoreAPI.Services.Employees
{
    public interface IEmployeeService
    {
        Task<EmployeeDetailsDto> GetEmployeeDataAsync();
    }

    public class EmployeeService
            (IUserContextService userContextService,
            UserManager<User> userManager) : IEmployeeService
    {
        public async Task<EmployeeDetailsDto> GetEmployeeDataAsync()
        {
            var user = await userContextService.GetUserAndCustomerDataByTokenAsync();
            EmployeeDetailsDto employeeDetails = new()
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Name = user.Customer.Name,
                Surname = user.Customer.Surname,

            };

            var userRoles = await userManager.GetRolesAsync(user);
            employeeDetails.RoleNames = userRoles.ToList();

            return employeeDetails;
        }
    }
}
