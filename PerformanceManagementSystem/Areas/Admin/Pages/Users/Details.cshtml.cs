using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PerformanceManagementSystem.Data.Models;

namespace PerformanceManagementSystem.Areas.Admin.Pages.Users
{
    public class DetailsModel : PageModel
    {
        private readonly PerformanceManagementSystem.Data.ApplicationDbContext _context;

        public DetailsModel(PerformanceManagementSystem.Data.ApplicationDbContext context)
        {
            _context = context;
        }

      public AppUser AppUser { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var appuser = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (appuser == null)
            {
                return NotFound();
            }
            else 
            {
                AppUser = appuser;
            }
            return Page();
        }
    }
}
