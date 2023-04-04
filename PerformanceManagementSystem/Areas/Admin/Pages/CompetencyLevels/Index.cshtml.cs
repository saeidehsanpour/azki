using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PerformanceManagementSystem.Data.Views.Competencies;
using PerformanceManagementSystem.Data.Views.CompetencyLevels;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace PerformanceManagementSystem.Areas.Admin.Pages.CompetencyLevels;

public class IndexModel : PageModel
{
    private readonly Data.ApplicationDbContext _context;
    private readonly IConfigurationProvider _configurationProvider;
    public IndexModel(Data.ApplicationDbContext context, IConfigurationProvider configurationProvider)
    {
        _context = context;
        _configurationProvider = configurationProvider;
    }

    public IList<CompetencyLevelResponseDto> CompetencyLevels { get; set; } = default!;
    public CompetencyResponseDto Competency { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(Guid competencyId)
    {
        if (competencyId != Guid.Empty)
        {
            var competency = await _context.Competencies
                .ProjectTo<CompetencyResponseDto>(_configurationProvider)
                .FirstOrDefaultAsync(a => a.Id == competencyId);

            if (competency == null)
                return RedirectToPage("/Admin/CompetencyLevelCategories");

            Competency = competency;

            CompetencyLevels = await _context.CompetencyLevels.Include(a => a.Competency).Where(a => a.CompetencyId == competencyId).ProjectTo<CompetencyLevelResponseDto>(_configurationProvider).ToListAsync();
            return Page();
        }

        return NotFound();
    }
}