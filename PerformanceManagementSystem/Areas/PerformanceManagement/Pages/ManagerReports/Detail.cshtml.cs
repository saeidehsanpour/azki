using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PerformanceManagementSystem.Data;
using PerformanceManagementSystem.Data.Enums;
using PerformanceManagementSystem.Data.Views.CompetencyLevels;
using PerformanceManagementSystem.Data.Views.Reports;
using PerformanceManagementSystem.Data.Views.TaskOfPeriods;

namespace PerformanceManagementSystem.Areas.PerformanceManagement.Pages.ManagerReports
{
    public class DetailModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public SelfReportManagerOverallTaskOfPeriodDetailResponseDto TaskOfPeriod { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
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
            var taskOfPeriod = await _context.TaskOfPeriods
                .Include(a => a.CompetencyLevelTaskMappings)
                .ThenInclude(a => a.CompetencyLevel.Competency)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.PerformanceManagementPeriodUserMappingId == performanceManagementPeriodUserMapping.Id
                && m.UserId == id && m.Type == TaskType.ManagerAssessmentOverall);

            if (taskOfPeriod == null)
            {
                return NotFound();
            }

            TaskOfPeriod = new SelfReportManagerOverallTaskOfPeriodDetailResponseDto
            {
                CompetencyLevels = taskOfPeriod.CompetencyLevelTaskMappings.Select(a => new CompetencyLevelSelectListResponseDto
                {
                    CompetencyId = a.CompetencyLevel.CompetencyId,
                    CompetencyLevelId = a.CompetencyLevelId,
                    CompetencyLevelName = a.CompetencyLevel.Description,
                    CompetencyName = a.CompetencyLevel.Competency.Title
                }).ToList(),
                Description = taskOfPeriod.Description,
                Fullname = $"{taskOfPeriod.User!.Firstname} {taskOfPeriod.User.Lastname}",
                Id = taskOfPeriod.Id,
                Continue = taskOfPeriod.Continue,
                RoleAndInfluence = taskOfPeriod.RoleAndInfluence,
                ShouldImprove = taskOfPeriod.ShouldImprove,
                SuccessRate = (SuccessRateRequestEnum)taskOfPeriod.SuccessRate
            };
            return Page();
        }
    }
}
