using System.ComponentModel.DataAnnotations;

namespace BookStoreDto.Dtos.Admin
{
    public class EmployeeDataEditDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string? PhoneNumber { get; set; }
        public List<string> RoleNames { get; set; }

        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
