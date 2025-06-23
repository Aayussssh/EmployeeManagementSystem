using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Pages.Employees
{
    public class UploadDocumentModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public UploadDocumentModel(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [BindProperty]
        public int EmployeeId { get; set; }

        [BindProperty]
        public IFormFile UploadedDocument { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            EmployeeId = id;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (UploadedDocument != null && UploadedDocument.Length > 0)
            {
                var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == EmployeeId);

                if (employee == null)
                    return NotFound();

                if (!string.IsNullOrEmpty(employee.DocumentPath))
                {
                    var oldFilePath = Path.Combine(_environment.WebRootPath, "uploads", employee.DocumentPath);
                    if (System.IO.File.Exists(oldFilePath))
                        System.IO.File.Delete(oldFilePath);
                }

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(UploadedDocument.FileName)}";
                var filePath = Path.Combine(_environment.WebRootPath, "uploads", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await UploadedDocument.CopyToAsync(stream);
                }

                employee.DocumentPath = fileName;
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Document uploaded successfully!";
                return RedirectToPage("Index");
            }

            TempData["ErrorMessage"] = "Please select a document to upload.";
            return Page();
        }
    }
}
