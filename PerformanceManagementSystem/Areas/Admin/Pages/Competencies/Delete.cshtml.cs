using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PerformanceManagementSystem.Data;
using PerformanceManagementSystem.Data.Models;

namespace PerformanceManagementSystem.Areas.Admin.Pages.Competencies;

public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DeleteModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Competency Competency { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var competency = await _context.Competencies.FirstOrDefaultAsync(m => m.Id == id);

        if (competency == null)
        {
            return NotFound();
        }

        Competency = competency;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var competency = await _context.Competencies.FindAsync(id);

        if (competency != null)
        {
            Competency = competency;
            _context.Competencies.Remove(Competency);
            await _context.SaveChangesAsync();
        }
        else
            return NotFound();

        return RedirectToPage("./Index", new { competencyCategoryId = competency.CompetencyCategoryId });
    }
}