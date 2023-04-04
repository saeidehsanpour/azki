using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PerformanceManagementSystem.Data;
using PerformanceManagementSystem.Data.Enums;
using PerformanceManagementSystem.Data.Models;
using PerformanceManagementSystem.Data.Views.CompetencyLevels;
using PerformanceManagementSystem.Data.Views.TaskOfPeriods;

namespace PerformanceManagementSystem.Areas.PerformanceManagement.Pages.ManagerAssessments;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;
    public EditModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> OnGetAsync(Guid taskId, Guid? id)
    {
        var isUserMentionInThisTask = await _context.TaskUserMentions.AnyAsync(a => a.UserId == HttpContext.User.UserId() && a.TaskOfPeriodId == taskId);

        if (!isUserMentionInThisTask)
        {
            return RedirectToPage("./Index");
        }

        var ownerTask = await _context.TaskOfPeriods
            .Include(a => a.CompetencyLevelTaskMappings)
            .ThenInclude(a => a.CompetencyLevel.Competency)
            .Include(a => a.PerformanceManagementPeriodUserMapping.User)
            .Select(a => new ManagerTaskOfPeriodResponseDto
            {
                TaskOwnerName = $"{a.PerformanceManagementPeriodUserMapping.User.Firstname} {a.PerformanceManagementPeriodUserMapping.User.Lastname} - {a.PerformanceManagementPeriodUserMapping.User.UserName}",
                Id = a.Id,
                Active = true,
                Description = a.Description,
                RoleAndInfluence = a.RoleAndInfluence,
                Title = a.Title,
                UpdatedDate = a.UpdatedDate,
                CompetencyNames = a.CompetencyLevelTaskMappings.Select(b => new CompetencyNames
                {
                    CompetencyLevelName = b.CompetencyLevel.Description,
                    CompetencyName = b.CompetencyLevel.Competency.Title
                }).ToList(),
                PositionId = a.PerformanceManagementPeriodUserMapping.User.PositionId
            })
            .FirstOrDefaultAsync(a => a.Id == taskId);

        if (ownerTask == null)
            return NotFound();

        TaskOfPeriodOfOwner = ownerTask;

        var positionId = ownerTask.PositionId;

        var competencies = await _context.CompetencyCategoryPositionMappings
            .Include(a => a.CompetencyCategory.Competencies)
            .Where(a => a.PositionId == positionId).Select(a => a.CompetencyCategory.Competencies).ToListAsync();

        List<object> com = new();
        foreach (var competency in competencies.Where(competency => competency.Any()))
        {
            com.AddRange(competency.Where(a => a.Active).Select(a => new
            {
                a.Id,
                a.Title
            }));
        }
        ViewData["Competencies"] = new SelectList(com, "Id", "Title");

        var taskOfPeers = await _context.TaskOfPeriods
            .Include(a => a.CompetencyLevelTaskMappings)
            .ThenInclude(a => a.CompetencyLevel.Competency)
            .Include(a => a.PerformanceManagementPeriodUserMapping.User)
            .Where(a => a.MainTaskOfPeriodId == taskId && a.Type == TaskType.PeerAssessment)
            .Select(a => new ManagerTaskOfPeriodResponseDto
            {
                TaskOwnerName = $"{a.User!.Firstname} {a.User.Lastname} - {a.User.UserName}",
                Id = a.Id,
                Active = true,
                Description = a.Description,
                RoleAndInfluence = a.RoleAndInfluence,
                Title = a.Title,
                UpdatedDate = a.UpdatedDate,
                CompetencyNames = a.CompetencyLevelTaskMappings.Select(b => new CompetencyNames
                {
                    CompetencyLevelName = b.CompetencyLevel.Description,
                    CompetencyName = b.CompetencyLevel.Competency.Title
                }).ToList()
            })
            .ToListAsync();
        TaskOfPeriodOfPeers = taskOfPeers;
        if (id != null || id != Guid.Empty)
        {
            var currentTaskOfPeriod = await _context.TaskOfPeriods
                .Include(a => a.CompetencyLevelTaskMappings)
                .ThenInclude(a => a.CompetencyLevel.Competency)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (currentTaskOfPeriod != null)
            {
                TaskOfPeriod = new PeerTaskOfPeriodRequestDto
                {
                    CompetencyLevelIds = currentTaskOfPeriod.CompetencyLevelTaskMappings.Select(a => a.CompetencyLevelId).ToList(),
                    RoleAndInfluence = currentTaskOfPeriod.RoleAndInfluence,
                    SuccessRate = currentTaskOfPeriod.SuccessRate,
                    Id = currentTaskOfPeriod.Id,
                    DutyInTask = currentTaskOfPeriod.DutyInTask,
                    MainTaskOfPeriodId = currentTaskOfPeriod.MainTaskOfPeriodId ?? taskId
                };
                CompetencyLevels = currentTaskOfPeriod.CompetencyLevelTaskMappings.Select(a => new CompetencyLevelSelectListResponseDto
                {
                    CompetencyId = a.CompetencyLevel.CompetencyId,
                    CompetencyLevelId = a.CompetencyLevelId,
                    CompetencyName = a.CompetencyLevel.Competency.Title,
                    CompetencyLevelName = a.CompetencyLevel.Description
                }).ToList();
                return Page();
            }
        }
        TaskOfPeriod = new PeerTaskOfPeriodRequestDto
        {
            CompetencyLevelIds = new List<Guid>(),
            RoleAndInfluence = string.Empty,
            SuccessRate = SuccessRateEnum.Normal,
            Id = Guid.Empty,
            DutyInTask = DutyEnum.Teammate,
            MainTaskOfPeriodId = taskId
        };
        return Page();
    }

    [BindProperty]
    public PeerTaskOfPeriodRequestDto TaskOfPeriod { get; set; } = default!;
    public ManagerTaskOfPeriodResponseDto TaskOfPeriodOfOwner { get; set; } = default!;
    public IList<ManagerTaskOfPeriodResponseDto> TaskOfPeriodOfPeers { get; set; } = default!;
    [BindProperty(SupportsGet = true)]
    public Guid CompetencyId { get; set; }
    public List<CompetencyLevelSelectListResponseDto> CompetencyLevels { get; set; } = new();

    public async Task<IActionResult> OnPostAsync(Guid taskId, Guid? id)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (id != null)
        {
            var currentTask = await _context.TaskOfPeriods.FindAsync(id);
            if (currentTask != null)
            {
                _context.TaskOfPeriods.Remove(currentTask);
            }
        }

        var task = await _context.TaskOfPeriods.FirstOrDefaultAsync(a => a.Id == taskId);

        if (task == null)
        {
            return RedirectToPage("./Index");
        }

        var newGuid = Guid.NewGuid();

        var taskOfPeriod = new TaskOfPeriod
        {
            Id = newGuid,
            Title = task.Title,
            Active = true,
            UserId = HttpContext.User.UserId(),
            DutyInTask = TaskOfPeriod.DutyInTask,
            RoleAndInfluence = TaskOfPeriod.RoleAndInfluence,
            SuccessRate = TaskOfPeriod.SuccessRate,
            Type = TaskType.ManagerAssessment,
            PerformanceManagementPeriodUserMappingId = task.PerformanceManagementPeriodUserMappingId,
            CompetencyLevelTaskMappings = TaskOfPeriod.CompetencyLevelIds.Select(competencyLevelId => new CompetencyLevelTaskMapping
            {
                Active = true,
                CompetencyLevelId = competencyLevelId,
                TaskOfPeriodId = newGuid,
                Title = string.Empty
            }).ToList(),
            MainTaskOfPeriodId = task.Id
        };

        await _context.TaskOfPeriods.AddAsync(taskOfPeriod);

        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
    public async Task<JsonResult> OnGetCompetencyLevels()
    {
        var competencyLevels = await _context.CompetencyLevels
            .Where(a => a.CompetencyId == CompetencyId).Select(a => new
            {
                a.Id,
                a.Title,
                a.Description
            }).ToListAsync();
        return new JsonResult(competencyLevels);
    }
    public class CompetencyLevel
    {
        public Guid CompetencyId { get; set; }
        public string? CompetencyName { get; set; }
        public Guid CompetencyLevelId { get; set; }
        public string? CompetencyLevelName { get; set; }
    }
}