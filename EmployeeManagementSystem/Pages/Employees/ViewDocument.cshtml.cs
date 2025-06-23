using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Pages.Employees
{
    public class ViewDocumentModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ViewDocumentModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public string DocumentPath { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null || string.IsNullOrEmpty(employee.DocumentPath))
            {
                return NotFound("Document not found.");
            }

            DocumentPath = employee.DocumentPath;
            return Page();
        }
    }
}
