using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagementSystem.Pages
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            if (User.IsInRole("Admin"))
            {
                return RedirectToPage("/Employees/Index");
            }
            else
            {
                return RedirectToPage("/Employees/MyDetails");
            }
        }
    }
}
