using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PerformanceManagementSystem.Data;
using PerformanceManagementSystem.Data.Views.Users;

namespace PerformanceManagementSystem.Areas.PerformanceManagement.Pages.ManagerEvaluations;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;
    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }
    public IList<ManagerResponseDto> Managers { get; set; } = default!;

    public async Task OnGetAsync()
    {
        var userId = HttpContext.User.UserId();
        Managers = await _context.UserManagerMappings
            .Include(a => a.Manager.Position)
            .Where(a => a.UserId == userId).Select(a => new ManagerResponseDto
            {
                Fullname = $"{a.Manager.Firstname} {a.Manager.Lastname}",
                Id = a.ManagerId,
                Position = a.Manager.Position.Title
            }).ToListAsync();
    }
}