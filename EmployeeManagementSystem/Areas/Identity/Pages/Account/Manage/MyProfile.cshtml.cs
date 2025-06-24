using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Areas.Identity.Pages.Account.Manage
{
    public class MyProfileModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _environment;
        private readonly ApplicationDbContext _context;

        public MyProfileModel(UserManager<ApplicationUser> userManager, IWebHostEnvironment environment, ApplicationDbContext context)
        {
            _userManager = userManager;
            _environment = environment;
            _context = context;
        }

        [BindProperty]
        public ApplicationUser AdminUser { get; set; }

        [BindProperty]
        public IFormFile? ProfileImage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            AdminUser = user;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (ProfileImage != null)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfileImage.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ProfileImage.CopyToAsync(fileStream);
                }

                var dbPath = "/uploads/" + uniqueFileName;

                // Update in AspNetUsers table
                user.ProfilePicturePath = dbPath;
                await _userManager.UpdateAsync(user);

                // Update in Employees table if the user is an Employee
                var employee = await _context.Employees.FirstOrDefaultAsync(e => e.UserId == user.Id);
                if (employee != null)
                {
                    employee.ProfilePicturePath = dbPath;
                    _context.Employees.Update(employee);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToPage();
        }
    }
}
