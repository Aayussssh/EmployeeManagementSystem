using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagementSystem.Areas.Identity.Pages.Account.Manage
{
    public class MyDetailsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public MyDetailsModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public ApplicationUser AdminUser { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("Admin user not found.");
            }

            AdminUser = user;
            return Page();
        }
    }
}
