using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PerformanceManagementSystem.Data;
using PerformanceManagementSystem.Data.Views.TaskOfPeriods;

namespace PerformanceManagementSystem.Areas.PerformanceManagement.Pages.PeerAssessments;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;
    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<PeerTaskOfPeriodListResponseDto> TaskOfPeriods { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = HttpContext.User.UserId();
        var timeId = HttpContext.Request.Cookies["PerformanceManagementCookie"];
        if (timeId == null)
        {
            return RedirectToPage("./Index");
        }
        var performanceManagementPeriodUserMapping = await _context.PerformanceManagementPeriodUserMappings
            .Where(a => a.PerformanceManagementPeriodId == Guid.Parse(timeId) && a.UserId == userId).FirstOrDefaultAsync();
        if (performanceManagementPeriodUserMapping == null)
        {
            return NotFound();
        }

        var taskOfPeriodUserMentions = await _context.TaskUserMentions
                .Include(a => a.TaskOfPeriod)
                .ThenInclude(a => a.PerformanceManagementPeriodUserMapping.User)
                .Where(a => a.UserId == userId && !a.Manager).Select(a => new PeerTaskOfPeriodListResponseDto
                {
                    MainTaskId = a.TaskOfPeriod.Id,
                    Active = a.Active,
                    Title = a.TaskOfPeriod.Title,
                    UpdatedDate = a.UpdatedDate,
                    PeerName = $"{a.TaskOfPeriod.PerformanceManagementPeriodUserMapping.User.Firstname} " +
                               $"{a.TaskOfPeriod.PerformanceManagementPeriodUserMapping.User.Lastname} - " +
                               $"{a.TaskOfPeriod.PerformanceManagementPeriodUserMapping.User.UserName}"
                }).ToListAsync();

        foreach (var item in taskOfPeriodUserMentions)
        {
            var submitted = await _context.TaskOfPeriods.FirstOrDefaultAsync(a =>
                a.MainTaskOfPeriodId == item.MainTaskId && a.UserId == userId);
            if (submitted != null)
            {
                item.Id = submitted.Id;
                item.SuccessRate = submitted.SuccessRate;
                item.DutyInTask = submitted.DutyInTask;
            }
        }

        TaskOfPeriods = taskOfPeriodUserMentions;

        return Page();
    }
}