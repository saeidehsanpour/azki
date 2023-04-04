using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PerformanceManagementSystem.Data;
using PerformanceManagementSystem.Data.Enums;
using PerformanceManagementSystem.Data.Models;
using PerformanceManagementSystem.Data.Views.CompetencyLevels;
using PerformanceManagementSystem.Data.Views.Reports;
using PerformanceManagementSystem.Data.Views.TaskOfPeriods;

namespace PerformanceManagementSystem.Areas.PerformanceManagement.Pages.SelfReports;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;
    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = HttpContext.User.UserId();
        var timeId = HttpContext.Request.Cookies["PerformanceManagementCookie"];
        if (timeId == null)
        {
            return RedirectToPage("./Index");
        }

        var performanceManagementPeriodUserMapping = await _context.PerformanceManagementPeriodUserMappings
            .Include(a => a.TaskOfPeriods)
            .ThenInclude(a => a.User)
            .Where(a => a.PerformanceManagementPeriodId == Guid.Parse(timeId) && a.UserId == userId).FirstOrDefaultAsync();

        if (performanceManagementPeriodUserMapping == null)
        {
            return NotFound();
        }

        TaskOfPeriods = performanceManagementPeriodUserMapping.TaskOfPeriods.Where(a => a.Type == TaskType.SelfAssessment)
            .Select(a => new SelfReportListResponseDto
            {
                Active = a.Active,
                Description = a.Description,
                Id = a.Id,
                RoleAndInfluence = a.RoleAndInfluence,
                SuccessRate = a.SuccessRate,
                Title = a.Title,
                UpdatedDate = a.UpdatedDate,
                Items = performanceManagementPeriodUserMapping.TaskOfPeriods.Where(b =>
                    b.Type is TaskType.PeerAssessment or TaskType.ManagerAssessment && b.MainTaskOfPeriodId == a.Id).Select(b => new SelfReportListItemDto
                {
                    Duty = b.DutyInTask,
                    Fullname = $"{b.User!.Firstname} {b.User.Lastname}",
                    Id = b.Id,
                    Manager = b.Type == TaskType.ManagerAssessment
                }).ToList()
            }).ToList();
        return Page();
    }

    public IList<SelfReportListResponseDto> TaskOfPeriods { get; set; } = default!;
}