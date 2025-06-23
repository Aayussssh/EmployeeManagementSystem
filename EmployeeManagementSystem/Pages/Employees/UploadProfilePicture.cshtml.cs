using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Pages.Employees
{
    public class UploadProfilePictureModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public UploadProfilePictureModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [BindProperty]
        public IFormFile ProfilePicture { get; set; } = default!;

        public string Message { get; set; } = string.Empty;

        public async Task<IActionResult> OnPostAsync()
        {
            if (ProfilePicture != null)
            {
                var userId = _userManager.GetUserId(User);
                var employee = await _context.Employees
                                 .FirstOrDefaultAsync(e => e.UserId == userId);

                if (employee == null)
                {
                    return NotFound();
                }

                var filePath = Path.Combine("wwwroot/uploads", ProfilePicture.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ProfilePicture.CopyToAsync(stream);
                }

                employee.ProfilePicturePath  = "/uploads/" + ProfilePicture.FileName;
                await _context.SaveChangesAsync();

                Message = "Profile picture uploaded successfully!";
            }
            else
            {
                Message = "Please select a picture.";
            }

            return Page();
        }
    }
}
