using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PerformanceManagementSystem.Data;
using PerformanceManagementSystem.Data.Views.Organizations;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace PerformanceManagementSystem.Areas.Admin.Pages.Organizations;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly IConfigurationProvider _configurationProvider;
    public IndexModel(ApplicationDbContext context, IConfigurationProvider configurationProvider)
    {
        _context = context;
        _configurationProvider = configurationProvider;
    }

    public IList<OrganizationResponseDto> Organizations { get; set; } = default!;

    public async Task OnGetAsync()
    {
        Organizations = await _context.Organizations
                .Include(o => o.AdminUser).ProjectTo<OrganizationResponseDto>(_configurationProvider).ToListAsync();
    }
}