using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PerformanceManagementSystem.Data;
using PerformanceManagementSystem.Data.Views.Organizations;

namespace PerformanceManagementSystem.Areas.Admin.Pages.Organizations
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public OrganizationResponseDto Organization { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var organization = await _context.Organizations.Select(a => new OrganizationResponseDto
            {
                Id = a.Id,
                AdminUserName = $"{a.AdminUser.Firstname} {a.AdminUser.Lastname}",
                Active = a.Active,
                AdminUserId = a.AdminUserId,
                Title = a.Title
            }).FirstOrDefaultAsync(m => m.Id == id);

            if (organization == null)
            {
                return NotFound();
            }

            Organization = organization;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var organization = await _context.Organizations.FindAsync(id);

            if (organization != null)
            {
                organization.Deleted = true;
                organization.UpdatedDate = DateTime.UtcNow;
                _context.Organizations.Update(organization);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
