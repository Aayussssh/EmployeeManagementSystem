using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Pages.Employees
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Employee Employee { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);

            if (Employee == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                return Page();
            }

            var employeeInDb = await _context.Employees.FirstOrDefaultAsync(e => e.Id == Employee.Id);

            if (employeeInDb == null)
            {
                return NotFound();
            }

            employeeInDb.Name = Employee.Name;
            employeeInDb.Position = Employee.Position;
            employeeInDb.Email = Employee.Email;
            employeeInDb.DateOfJoining = DateTime.SpecifyKind(Employee.DateOfJoining, DateTimeKind.Utc);
            employeeInDb.Salary = Employee.Salary;

            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
