using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace EmployeeManagementSystem.Pages.Employees
{
    public class MyDetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MyDetailsModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Employee Employee { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);
            Employee = await _context.Employees.FirstOrDefaultAsync(e => e.UserId == userId);

            if (Employee == null)
            {
                return NotFound("Employee details not found.");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = _userManager.GetUserId(User);
            var employeeInDb = await _context.Employees.FirstOrDefaultAsync(e => e.UserId == userId);

            if (employeeInDb == null)
            {
                return NotFound("Employee details not found.");
            }

            employeeInDb.Name = Employee.Name;
            employeeInDb.Position = Employee.Position;
            employeeInDb.Email = Employee.Email;
            employeeInDb.DateOfJoining = DateTime.SpecifyKind(Employee.DateOfJoining, DateTimeKind.Utc);
            employeeInDb.Salary = Employee.Salary;

            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}
