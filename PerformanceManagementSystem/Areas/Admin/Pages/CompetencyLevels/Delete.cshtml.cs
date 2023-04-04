using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PerformanceManagementSystem.Data;
using PerformanceManagementSystem.Data.Models;

namespace PerformanceManagementSystem.Areas.Admin.Pages.CompetencyLevels;

public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DeleteModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public CompetencyLevel CompetencyLevel { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var competency = await _context.CompetencyLevels.FirstOrDefaultAsync(m => m.Id == id);

        if (competency == null)
        {
            return NotFound();
        }

        CompetencyLevel = competency;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var competency = await _context.CompetencyLevels.FindAsync(id);

        if (competency != null)
        {
            CompetencyLevel = competency;
            _context.CompetencyLevels.Remove(CompetencyLevel);
            await _context.SaveChangesAsync();
        }
        else
            return NotFound();

        return RedirectToPage("./Index", new { competencyId = competency.CompetencyId });
    }
}