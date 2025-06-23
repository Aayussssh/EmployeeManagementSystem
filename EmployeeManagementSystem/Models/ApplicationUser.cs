using Microsoft.AspNetCore.Identity;

namespace EmployeeManagementSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? ProfilePicturePath { get; set; }
        public bool IsAdmin { get; set; }
    }
}