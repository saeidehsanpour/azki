using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PerformanceManagementSystem.Data;
using PerformanceManagementSystem.Data.Enums;
using PerformanceManagementSystem.Data.Models;
using PerformanceManagementSystem.Data.Views.Competencies;
using PerformanceManagementSystem.Data.Views.CompetencyLevels;
using PerformanceManagementSystem.Data.Views.TaskOfPeriods;

namespace PerformanceManagementSystem.Areas.PerformanceManagement.Pages.ManagerAssessments;

public class OverallModel : PageModel
{
    private readonly ApplicationDbContext _context;
    public OverallModel(ApplicationDbContext context)
    {
        _context = context;
    }
    [BindProperty]
    public TaskOfPeriodOverallManagerRequestDto TaskOfPeriodOverall { get; set; } = default!;
    public IList<TaskOfPeriodWithCompetencyResponseDto> TaskOfPeriods { get; set; } = default!;
    public IList<CompetencyWithLevelsResponseDto> CompetencyLevelsNotSubmitted { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            return RedirectToPage("./Index");
        }

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

        var currentUser = await _context.Users
            .Where(a => a.Id == userId)
            .Select(a => new
            {
                a.Id,
                a.PositionId
            }).ToListAsync();

        if (!currentUser.Any())
        {
            return NotFound();
        }

        var competencyLevelTaskMappings = await _context.CompetencyLevelTaskMappings
            .Include(a => a.CompetencyLevel.Competency)
            .Include(a => a.TaskOfPeriod)
            .Where(a => a.TaskOfPeriod.PerformanceManagementPeriodUserMappingId ==
                        performanceManagementPeriodUserMapping.Id && a.TaskOfPeriod.UserId == HttpContext.User.UserId() && a.TaskOfPeriod.Type == TaskType.ManagerAssessment).ToListAsync();

        var groupByCompetency = competencyLevelTaskMappings.GroupBy(a => a.CompetencyLevel.CompetencyId)
            .Select(a => new { Id = a.Key, Values = a.ToList() });
        TaskOfPeriods = new List<TaskOfPeriodWithCompetencyResponseDto>();
        foreach (var item in groupByCompetency)
        {
            var competencyLevelTaskMapping = item.Values.MaxBy(a => a.CompetencyLevel.Level);
            if (competencyLevelTaskMapping != null)
            {
                TaskOfPeriods.Add(new TaskOfPeriodWithCompetencyResponseDto
                {
                    Id = competencyLevelTaskMapping.CompetencyLevel.CompetencyId,
                    CompetencyLevel = competencyLevelTaskMapping.CompetencyLevel.Description ?? string.Empty,
                    Title = competencyLevelTaskMapping.CompetencyLevel.Competency.Title
                });
            }
        }

        var competencies = await _context.CompetencyCategoryPositionMappings
            .Include(a => a.CompetencyCategory.Competencies)
            .ThenInclude(a => a.CompetencyLevels)
            .Where(a => a.PositionId == currentUser[0].PositionId)
            .SelectMany(a => a.CompetencyCategory.Competencies).ToListAsync();

        List<Competency> competencyLevelsNotSubmitted = competencies.Where(a => a.Active).ToList();

        List<Competency> shouldRemove = competencyLevelsNotSubmitted
            .Where(competency => TaskOfPeriods.Any(a => a.Id == competency.Id)).ToList();

        competencyLevelsNotSubmitted = competencyLevelsNotSubmitted.Except(shouldRemove).OrderBy(a => a.Title).ToList();

        var currentSelfAssessmentOverall = await _context.TaskOfPeriods
            .Include(a => a.CompetencyLevelTaskMappings)
            .ThenInclude(a => a.CompetencyLevel.Competency)
            .FirstOrDefaultAsync(a => a.PerformanceManagementPeriodUserMappingId
                                      == performanceManagementPeriodUserMapping.Id
                                      && a.UserId == HttpContext.User.UserId() && a.Type == TaskType.ManagerAssessmentOverall);
        if (currentSelfAssessmentOverall != null)
        {
            TaskOfPeriodOverall = new TaskOfPeriodOverallManagerRequestDto
            {
                Id = currentSelfAssessmentOverall.Id,
                PerformanceManagementPeriodUserMappingId = performanceManagementPeriodUserMapping.Id,
                Continue = currentSelfAssessmentOverall.Continue ?? string.Empty,
                Description = currentSelfAssessmentOverall.Description ?? string.Empty,
                ShouldImprove = currentSelfAssessmentOverall.ShouldImprove ?? string.Empty,
                RoleAndInfluence = currentSelfAssessmentOverall.RoleAndInfluence ?? string.Empty,
                SuccessRate = (SuccessRateRequestEnum)currentSelfAssessmentOverall.SuccessRate,
                CompetencyLevelTaskMappings = currentSelfAssessmentOverall.CompetencyLevelTaskMappings.Where(a => a.Active).Select(b => new CompetencyLevelTaskMappingRequestDto
                {
                    Description = b.Title,
                    CompetencyLevelId = b.CompetencyLevelId
                }).ToList()
            };
        }
        else
        {
            TaskOfPeriodOverall = new TaskOfPeriodOverallManagerRequestDto
            {
                PerformanceManagementPeriodUserMappingId = performanceManagementPeriodUserMapping.Id,
                Continue = string.Empty,
                Description = string.Empty,
                ShouldImprove = string.Empty,
                RoleAndInfluence = string.Empty,
                SuccessRate = SuccessRateRequestEnum.A,
                CompetencyLevelTaskMappings = competencyLevelsNotSubmitted.Where(a => a.Active).Select(_ => new CompetencyLevelTaskMappingRequestDto
                {
                    Description = string.Empty,
                    CompetencyLevelId = Guid.Empty
                }).ToList()
            };
        }
        CompetencyLevelsNotSubmitted = competencyLevelsNotSubmitted.Where(a => a.Active).Select(a => new CompetencyWithLevelsResponseDto
        {
            Title = a.Title,
            Id = a.Id,
            Active = a.Active,
            Description = a.Description,
            UpdatedDate = a.UpdatedDate,
            Levels = a.CompetencyLevels.Select(b => new CompetencyLevelResponseDto
            {
                Id = b.Id,
                Title = b.Title,
                Active = b.Active,
                Description = b.Description,
                CompetencyId = b.CompetencyId,
                UpdatedDate = b.UpdatedDate,
                Level = b.Level
            }).ToList()
        }).ToList();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (TaskOfPeriodOverall.Id != Guid.Empty)
        {

            var currentSelfAssessmentOverall = await _context.TaskOfPeriods
                .FirstOrDefaultAsync(a => a.Id == TaskOfPeriodOverall.Id);
            if (currentSelfAssessmentOverall != null)
            {
                _context.TaskOfPeriods.Remove(currentSelfAssessmentOverall);
            }
        }
        var id = Guid.NewGuid();

        var taskOfPeriod = new TaskOfPeriod
        {
            Id = id,
            Title = string.Empty,
            Active = true,
            Description = TaskOfPeriodOverall.Description,
            UserId = HttpContext.User.UserId(),
            Continue = TaskOfPeriodOverall.Continue,
            ShouldImprove = TaskOfPeriodOverall.ShouldImprove,
            Type = TaskType.ManagerAssessmentOverall,
            SuccessRate = (SuccessRateEnum)TaskOfPeriodOverall.SuccessRate,
            PerformanceManagementPeriodUserMappingId = TaskOfPeriodOverall.PerformanceManagementPeriodUserMappingId,
            RoleAndInfluence = TaskOfPeriodOverall.RoleAndInfluence,
            CompetencyLevelTaskMappings = TaskOfPeriodOverall.CompetencyLevelTaskMappings.Select(competencyLevelTaskMapping => new CompetencyLevelTaskMapping
            {
                Active = true,
                CompetencyLevelId = competencyLevelTaskMapping.CompetencyLevelId,
                TaskOfPeriodId = id,
                Title = competencyLevelTaskMapping.Description ?? string.Empty
            }).ToList()
        };

        await _context.TaskOfPeriods.AddAsync(taskOfPeriod);

        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}