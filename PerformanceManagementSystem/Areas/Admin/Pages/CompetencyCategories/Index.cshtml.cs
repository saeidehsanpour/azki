using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PerformanceManagementSystem.Data.Views.CompetencyCategories;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace PerformanceManagementSystem.Areas.Admin.Pages.CompetencyCategories;

public class IndexModel : PageModel
{
    private readonly Data.ApplicationDbContext _context;
    private readonly IConfigurationProvider _configurationProvider;
    public IndexModel(Data.ApplicationDbContext context, IConfigurationProvider configurationProvider)
    {
        _context = context;
        _configurationProvider = configurationProvider;
    }

    public IList<CompetencyCategoryResponseDto> CompetencyCategories { get; set; } = default!;

    public async Task OnGetAsync()
    {
        if (_context.Users != null)
        {
            CompetencyCategories = await _context.CompetencyCategories.ProjectTo<CompetencyCategoryResponseDto>(_configurationProvider).ToListAsync();
        }
    }
}