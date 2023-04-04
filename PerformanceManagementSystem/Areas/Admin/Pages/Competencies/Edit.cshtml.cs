using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PerformanceManagementSystem.Data;
using PerformanceManagementSystem.Data.Models;
using PerformanceManagementSystem.Data.Views.Competencies;

namespace PerformanceManagementSystem.Areas.Admin.Pages.Competencies;

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
    public CompetencyRequestDto Competency { get; set; } = default!;
    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var competency = await _context.Competencies.Select(a => new CompetencyRequestDto
        {
            Id = a.Id,
            Active = a.Active,
            Description = a.Description,
            Title = a.Title,
            CompetencyCategoryId = a.CompetencyCategoryId,
        }).FirstOrDefaultAsync(m => m.Id == id);
        if (competency == null)
        {
            return NotFound();
        }
        Competency = competency;
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

        var competency = _mapper.Map<CompetencyRequestDto, Competency>(Competency);
        competency.UpdatedDate = DateTimeOffset.UtcNow;
        _context.Competencies.Update(competency);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CompetencyExists(Competency.Id))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToPage("./Index", new { competencyCategoryId = competency.CompetencyCategoryId });
    }

    private bool CompetencyExists(Guid id)
    {
        return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}