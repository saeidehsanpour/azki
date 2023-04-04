using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PerformanceManagementSystem.Data;
using PerformanceManagementSystem.Data.Views.CompetencyLevels;
using PerformanceManagementSystem.Data.Views.Reports;

namespace PerformanceManagementSystem.Areas.PerformanceManagement.Pages.SelfReports
{
    public class DetailModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public SelfReportTaskOfPeriodDetailResponseDto TaskOfPeriod { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskOfPeriod = await _context.TaskOfPeriods
                .Include(a => a.CompetencyLevelTaskMappings)
                .ThenInclude(a => a.CompetencyLevel.Competency)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskOfPeriod == null)
            {
                return NotFound();
            }

            TaskOfPeriod = new SelfReportTaskOfPeriodDetailResponseDto
            {
                CompetencyLevels = taskOfPeriod.CompetencyLevelTaskMappings.Select(a => new CompetencyLevelSelectListResponseDto
                {
                    CompetencyId = a.CompetencyLevel.CompetencyId,
                    CompetencyLevelId = a.CompetencyLevelId,
                    CompetencyLevelName = a.CompetencyLevel.Description,
                    CompetencyName = a.CompetencyLevel.Competency.Title
                }).ToList(),
                Description = taskOfPeriod.Description ?? string.Empty,
                Fullname = $"{taskOfPeriod.User!.Firstname} {taskOfPeriod.User.Lastname}",
                Id = taskOfPeriod.Id,
                Title = taskOfPeriod.Title,
                SuccessRate = taskOfPeriod.SuccessRate,
                Duty = taskOfPeriod.DutyInTask
            };
            return Page();
        }
    }
}
