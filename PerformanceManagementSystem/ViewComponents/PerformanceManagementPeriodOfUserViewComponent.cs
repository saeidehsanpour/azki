using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerformanceManagementSystem.Data;
using PerformanceManagementSystem.Data.Views.PerformanceManagementPeriods;

namespace PerformanceManagementSystem.ViewComponents;

public class PerformanceManagementPeriodOfUserViewComponent : ViewComponent
{
    private readonly ApplicationDbContext _context;

    public PerformanceManagementPeriodOfUserViewComponent(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var userId = HttpContext.User.UserId();
        var news = await _context.PerformanceManagementPeriodUserMappings
            .Include(a => a.PerformanceManagementPeriod)
            .Where(a => a.UserId == userId && a.PerformanceManagementPeriod.Active)
            .Select(a => new PerformanceManagementPeriodResponseDto
            {
                Title = a.PerformanceManagementPeriod.Title,
                Active = a.Active,
                Id = a.PerformanceManagementPeriodId,
                UpdatedDate = a.UpdatedDate
            })
            .ToListAsync();
        return await Task.Run(() => View(news));
    }
}