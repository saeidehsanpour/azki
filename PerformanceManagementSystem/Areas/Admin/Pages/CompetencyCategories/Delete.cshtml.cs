using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PerformanceManagementSystem.Data;
using PerformanceManagementSystem.Data.Models;

namespace PerformanceManagementSystem.Areas.Admin.Pages.CompetencyCategories;

public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DeleteModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public CompetencyCategory CompetencyCategory { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var competencyCategory = await _context.CompetencyCategories.FirstOrDefaultAsync(m => m.Id == id);

        if (competencyCategory == null)
        {
            return NotFound();
        }

        CompetencyCategory = competencyCategory;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var competencyCategory = await _context.CompetencyCategories.FindAsync(id);

        if (competencyCategory != null)
        {
            CompetencyCategory = competencyCategory;
            _context.CompetencyCategories.Remove(CompetencyCategory);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}