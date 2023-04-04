using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PerformanceManagementSystem.Data.Views;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace PerformanceManagementSystem.Areas.Admin.Pages.Users;

public class IndexModel : PageModel
{
    private readonly Data.ApplicationDbContext _context;
    private readonly IConfigurationProvider _configurationProvider;
    public IndexModel(Data.ApplicationDbContext context, IConfigurationProvider configurationProvider)
    {
        _context = context;
        _configurationProvider = configurationProvider;
    }

    public IList<UserResponseDto> AppUser { get;set; } = default!;

    public async Task OnGetAsync()
    {
        if (_context.Users != null)
        {
            AppUser = await _context.Users.ProjectTo<UserResponseDto>(_configurationProvider).ToListAsync();
        }
    }
}