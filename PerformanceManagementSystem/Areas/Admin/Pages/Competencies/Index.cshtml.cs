using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PerformanceManagementSystem.Data.Views.Competencies;
using PerformanceManagementSystem.Data.Views.CompetencyCategories;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace PerformanceManagementSystem.Areas.Admin.Pages.Competencies;

public class IndexModel : PageModel
{
    private readonly Data.ApplicationDbContext _context;
    private readonly IConfigurationProvider _configurationProvider;
    public IndexModel(Data.ApplicationDbContext context, IConfigurationProvider configurationProvider)
    {
        _context = context;
        _configurationProvider = configurationProvider;
    }

    public IList<CompetencyResponseDto> Competencies { get; set; } = default!;
    public CompetencyCategoryResponseDto CompetencyCategory { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(Guid competencyCategoryId)
    {
        if (competencyCategoryId != Guid.Empty)
        {
            var competencyCategory = await _context.CompetencyCategories
                .ProjectTo<CompetencyCategoryResponseDto>(_configurationProvider)
                .FirstOrDefaultAsync(a => a.Id == competencyCategoryId);

            if (competencyCategory == null)
                return RedirectToPage("/Admin/CompetencyCategories");

            CompetencyCategory = competencyCategory;

            Competencies = await _context.Competencies.Include(a => a.CompetencyCategory).Where(a => a.CompetencyCategoryId == competencyCategoryId).ProjectTo<CompetencyResponseDto>(_configurationProvider).ToListAsync();
            return Page();
        }

        return NotFound();
    }
}