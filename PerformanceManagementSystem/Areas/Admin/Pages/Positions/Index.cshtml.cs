using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PerformanceManagementSystem.Data.Views.Organizations;
using PerformanceManagementSystem.Data.Views.Positions;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace PerformanceManagementSystem.Areas.Admin.Pages.Positions;

public class IndexModel : PageModel
{
    private readonly Data.ApplicationDbContext _context;
    private readonly IConfigurationProvider _configurationProvider;
    public IndexModel(Data.ApplicationDbContext context, IConfigurationProvider configurationProvider)
    {
        _context = context;
        _configurationProvider = configurationProvider;
    }

    public IList<PositionResponseDto> Positions { get; set; } = default!;
    public OrganizationResponseDto Organization { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(Guid organizationId)
    {
        if (organizationId != Guid.Empty)
        {
            var organization = await _context.Organizations
                .ProjectTo<OrganizationResponseDto>(_configurationProvider)
                .FirstOrDefaultAsync(a => a.Id == organizationId);

            if (organization == null)
                return RedirectToPage("/Admin/Organizations/Index");

            Organization = organization;

            Positions = await _context.Positions.Include(a => a.Organization)
                .Where(a => a.OrganizationId == organizationId)
                .ProjectTo<PositionResponseDto>(_configurationProvider).ToListAsync();
            return Page();
        }

        return NotFound();
    }
}