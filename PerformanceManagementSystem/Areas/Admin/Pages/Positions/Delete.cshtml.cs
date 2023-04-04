using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PerformanceManagementSystem.Data;
using PerformanceManagementSystem.Data.Models;

namespace PerformanceManagementSystem.Areas.Admin.Pages.Positions;

public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DeleteModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Position Position { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var position = await _context.Positions.FirstOrDefaultAsync(m => m.Id == id);

        if (position == null)
        {
            return NotFound();
        }

        Position = position;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var competency = await _context.Positions.FindAsync(id);

        if (competency != null)
        {
            Position = competency;
            _context.Positions.Remove(Position);
            await _context.SaveChangesAsync();
        }
        else
            return NotFound();

        return RedirectToPage("./Index", new { organizationId = competency.OrganizationId });
    }
}