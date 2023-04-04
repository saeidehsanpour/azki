using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PerformanceManagementSystem.Data;
using PerformanceManagementSystem.Data.Models;
using PerformanceManagementSystem.Data.Views.CompetencyCategories;
using PerformanceManagementSystem.Data.Views.CompetencyCategoryPositionMappings;

namespace PerformanceManagementSystem.Areas.Admin.Pages.Positions;

public class CompetencyMappingModel : PageModel
{
    private readonly ApplicationDbContext _context;
    public CompetencyMappingModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public CompetencyCategoryPositionMappingRequestDto CompetencyCategoryPositionMapping { get; set; } = default!;
    [BindProperty]
    public Guid OrganizationId { get; set; }
    public async Task<IActionResult> OnGetAsync(Guid? positionId)
    {
        if (positionId == null)
        {
            return NotFound();
        }
        ViewData["CompetencyCategories"] = new SelectList(_context.CompetencyCategories.Where(a => a.Active)
            .Select(a => new CompetencyCategoryResponseDto
            {
                Id = a.Id,
                Title = a.Title
            }), "Id", "Title");
        var position = await _context.Positions.FirstOrDefaultAsync(m => m.Id == positionId && m.Active);
        if (position == null)
        {
            return NotFound();
        }

        var competencyCategoryPositionMapping = await _context.CompetencyCategoryPositionMappings
            .Where(a => a.PositionId == positionId).Select(a => a.CompetencyCategoryId).ToListAsync();
        OrganizationId = position.OrganizationId;
        CompetencyCategoryPositionMapping = new CompetencyCategoryPositionMappingRequestDto
        {
            PositionId = positionId.Value,
            CompetencyCategoryIds = competencyCategoryPositionMapping
        };
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var competencyCategoryPositionMappings = await _context.CompetencyCategoryPositionMappings
            .Where(a => a.PositionId == CompetencyCategoryPositionMapping.PositionId).ToListAsync();

        _context.CompetencyCategoryPositionMappings.RemoveRange(competencyCategoryPositionMappings);

        foreach (var newCompetencyCategoryPositionMappings in CompetencyCategoryPositionMapping.CompetencyCategoryIds.Select(competencyCategoryId => new CompetencyCategoryPositionMapping
                 {
                     CompetencyCategoryId = competencyCategoryId,
                     Active = true,
                     Title = string.Empty,
                     PositionId = CompetencyCategoryPositionMapping.PositionId
                 }))
        {
            await _context.CompetencyCategoryPositionMappings.AddAsync(newCompetencyCategoryPositionMappings);
        }

        await _context.SaveChangesAsync();


        return RedirectToPage("./Index", new { organizationId = OrganizationId });
    }
}