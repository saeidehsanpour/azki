using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PerformanceManagementSystem.Data;
using PerformanceManagementSystem.Data.Models;
using PerformanceManagementSystem.Data.Views.CompetencyLevels;

namespace PerformanceManagementSystem.Areas.Admin.Pages.CompetencyLevels;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    public EditModel(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [BindProperty]
    public CompetencyLevelRequestDto CompetencyLevel { get; set; } = default!;
    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var competencyLevel = await _context.CompetencyLevels.Select(a => new CompetencyLevelRequestDto
        {
            Id = a.Id,
            Active = a.Active,
            Description = a.Description,
            Title = a.Title,
            CompetencyId = a.CompetencyId,
        }).FirstOrDefaultAsync(m => m.Id == id);
        if (competencyLevel == null)
        {
            return NotFound();
        }
        CompetencyLevel = competencyLevel;
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

        var competencyLevel = _mapper.Map<CompetencyLevelRequestDto, CompetencyLevel>(CompetencyLevel);
        competencyLevel.UpdatedDate = DateTimeOffset.UtcNow;
        _context.CompetencyLevels.Update(competencyLevel);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CompetencyLevelExists(CompetencyLevel.Id))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToPage("./Index", new { competencyId = competencyLevel.CompetencyId });
    }

    private bool CompetencyLevelExists(Guid id)
    {
        return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}