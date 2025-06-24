using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
            try
            {
                var employee = await _context.Employees.FindAsync(id);

                if (employee == null)
                {
                    TempData["ErrorMessage"] = "Employee not found.";
                    return RedirectToPage("/Employees/Index");
                }

                if (string.IsNullOrEmpty(employee.DocumentPath))
                {
                    TempData["ErrorMessage"] = "No document uploaded for this employee.";
                    return RedirectToPage("/Employees/Index");
                }

                DocumentPath = employee.DocumentPath;
                return Page();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while retrieving the document.";
                return RedirectToPage("/Employees/Index");
            }
        }
    }
}
