using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PerformanceManagementSystem.Data;
using PerformanceManagementSystem.Data.Views.UserExceptions;

namespace PerformanceManagementSystem.Areas.PerformanceManagement.Pages.Exceptions;

public class DetailModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DetailModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public UserExceptionResponseDto UserException { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid? managerId)
    {
        if (managerId == null)
        {
            return NotFound();
        }

        var userId = HttpContext.User.UserId();
        var timeId = HttpContext.Request.Cookies["PerformanceManagementCookie"];
        if (timeId == null)
        {
            return RedirectToPage("./Index");
        }

        var performanceManagementPeriodUserMapping = await _context.PerformanceManagementPeriodUserMappings
            .FirstOrDefaultAsync(a => a.PerformanceManagementPeriodId == Guid.Parse(timeId) && a.UserId == userId);

        if (performanceManagementPeriodUserMapping == null)
        {
            return NotFound();
        }
        var userException = await _context.UserExceptions
            .Include(a => a.UserExceptionCompetencyMappings)
            .ThenInclude(a => a.Competency)
            .Include(a => a.Manager)
            .FirstOrDefaultAsync(m => m.PerformanceManagementPeriodUserMappingId == performanceManagementPeriodUserMapping.Id
                                      && m.ManagerId == managerId);

        if (userException == null)
        {
            return NotFound();
        }

        UserException = new UserExceptionResponseDto
        {
            Id = userException.Id,
            Title = userException.Title,
            Active = userException.Active,
            UpdatedDate = userException.UpdatedDate,
            Fullname = $"{userException.Manager.Firstname} {userException.Manager.Lastname}",
            Continue = userException.Continue,
            Items = userException.UserExceptionCompetencyMappings.Select(a => new UserExceptionItemResponseDto
            {
                Description = a.Description ?? string.Empty,
                CompetencyName = a.Competency.Title
            }).ToList()
        };
        return Page();
    }
}