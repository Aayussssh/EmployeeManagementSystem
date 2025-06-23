using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models
{
    public class Employee
    {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Position { get; set; }
        public string? ProfilePicturePath { get; set; }
        public string? DocumentPath { get; set; }
        public string? UserId { get; set; }
        public string Email { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string? Salary { get; set; }
        public ApplicationUser User { get; set; }
    }
}