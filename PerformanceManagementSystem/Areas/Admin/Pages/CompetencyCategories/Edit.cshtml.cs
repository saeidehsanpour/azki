using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PerformanceManagementSystem.Data;
using PerformanceManagementSystem.Data.Models;
using PerformanceManagementSystem.Data.Views.CompetencyCategories;

namespace PerformanceManagementSystem.Areas.Admin.Pages.CompetencyCategories;

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
    public CompetencyCategoryRequestDto CompetencyCategory { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var competencyCategory = await _context.CompetencyCategories.Select(a => new CompetencyCategoryRequestDto
        {
            Id = a.Id,
            Active = a.Active,
            Description = a.Description,
            Title = a.Title
        }).FirstOrDefaultAsync(m => m.Id == id);
        if (competencyCategory == null)
        {
            return NotFound();
        }
        CompetencyCategory = competencyCategory;
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

        var competencyCategory = _mapper.Map<CompetencyCategoryRequestDto, CompetencyCategory>(CompetencyCategory);
        competencyCategory.UpdatedDate = DateTimeOffset.UtcNow;
        _context.CompetencyCategories.Update(competencyCategory);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CompetencyCategoryExists(CompetencyCategory.Id))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToPage("./Index");
    }

    private bool CompetencyCategoryExists(Guid id)
    {
        return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}