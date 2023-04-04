using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PerformanceManagementSystem.Data;
using PerformanceManagementSystem.Data.Enums;
using PerformanceManagementSystem.Data.Views.TaskOfPeriods;

namespace PerformanceManagementSystem.Areas.PerformanceManagement.Pages.SelfAssessments;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;
    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<TaskOfPeriodListResponseDto> TaskOfPeriods { get; set; } = default!;
    public bool EndDate { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = HttpContext.User.UserId();
        var timeId = HttpContext.Request.Cookies["PerformanceManagementCookie"];
        if (timeId == null)
        {
            return RedirectToPage("./Index");
        }
        var performanceManagementPeriodUserMapping = await _context.PerformanceManagementPeriodUserMappings
            .Include(a => a.PerformanceManagementPeriod)
            .Where(a => a.PerformanceManagementPeriodId == Guid.Parse(timeId) && a.UserId == userId).FirstOrDefaultAsync();
        if (performanceManagementPeriodUserMapping == null)
        {
            return NotFound();
        }
        
        EndDate = performanceManagementPeriodUserMapping.PerformanceManagementPeriod.SelfScoreEndDate.Date < DateTime.UtcNow.Date;
        TaskOfPeriods = await _context.TaskOfPeriods
                .Include(o => o.TaskUserMentions)
                .Include(a => a.CompetencyLevelTaskMappings)
                .Where(a => a.PerformanceManagementPeriodUserMappingId == performanceManagementPeriodUserMapping.Id && a.Type == TaskType.SelfAssessment)
                .Select(a => new TaskOfPeriodListResponseDto
                {
                    Id = a.Id,
                    Active = a.Active,
                    NumberOfCompetencyLinked = a.CompetencyLevelTaskMappings.Count,
                    NumberOfMentionUsers = a.TaskUserMentions.Where(b => !b.Manager).ToList().Count,
                    SuccessRate = a.SuccessRate,
                    Title = a.Title,
                    UpdatedDate = a.UpdatedDate
                }).ToListAsync();

        return Page();
    }
}