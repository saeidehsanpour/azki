using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PerformanceManagementSystem.Data;
using PerformanceManagementSystem.Data.Enums;
using PerformanceManagementSystem.Data.Models;
using PerformanceManagementSystem.Data.Views;
using PerformanceManagementSystem.Data.Views.CompetencyLevels;
using PerformanceManagementSystem.Data.Views.TaskOfPeriods;

namespace PerformanceManagementSystem.Areas.PerformanceManagement.Pages.SelfAssessments;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;
    public EditModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        var timeId = HttpContext.Request.Cookies["PerformanceManagementCookie"];
        if (timeId == null)
        {
            return RedirectToPage("./Index");
        }

        var currentUser = await _context.Users
            .Include(a => a.Managers)
            .ThenInclude(a => a.User)
            .Where(a => a.Id == HttpContext.User.UserId())
            .Select(a => new
            {
                a.Id,
                a.PositionId,
                Manager = a.Managers.Select(b => b.Manager),
            }).FirstOrDefaultAsync();
        if (currentUser == null)
        {
            return NotFound();
        }

        var performanceManagementPeriodUserMapping = await _context.PerformanceManagementPeriodUserMappings
            .Include(a => a.PerformanceManagementPeriod)
            .Where(a => a.PerformanceManagementPeriodId == Guid.Parse((ReadOnlySpan<char>)timeId)
                        && a.UserId == currentUser.Id).FirstOrDefaultAsync();

        if (performanceManagementPeriodUserMapping == null
            || performanceManagementPeriodUserMapping.PerformanceManagementPeriod.SelfScoreEndDate.Date < DateTime.UtcNow.Date)
        {
            return RedirectToPage("./Index");
        }

        var currentTaskOfPeriod = await _context.TaskOfPeriods
            .Include(a => a.CompetencyLevelTaskMappings)
            .ThenInclude(a => a.CompetencyLevel.Competency)
            .Include(a => a.TaskUserMentions)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (currentTaskOfPeriod == null)
        {
            return NotFound();
        }

        ViewData["TaskUserMentionIds"] = new SelectList(_context.Users.Where(a => a.PositionId == currentUser.PositionId && a.Id != currentUser.Id).Select(a => new UserResponseDto
        {
            Id = a.Id,
            Name = $"{a.Firstname} {a.Lastname} ({a.Email})"
        }), "Id", "Name", currentTaskOfPeriod.TaskUserMentions.Select(a => a.UserId).ToList());

        var competencies = await _context.CompetencyCategoryPositionMappings
            .Include(a => a.CompetencyCategory.Competencies)
            .Where(a => a.PositionId == currentUser.PositionId).Select(a => a.CompetencyCategory.Competencies).ToListAsync();

        List<object> com = new List<object>();
        foreach (var competency in competencies.Where(competency => competency.Any()))
        {
            com.AddRange(competency.Where(a => a.Active).Select(a => new
            {
                a.Id,
                a.Title
            }));
        }
        ViewData["Competencies"] = new SelectList(com, "Id", "Title");

        ManagerNames = currentUser.Manager.Select(a => $"{a.Firstname} {a.Lastname}").ToList();
        TaskOfPeriod = new TaskOfPeriodRequestDto
        {
            Id = currentTaskOfPeriod.Id,
            Description = currentTaskOfPeriod.Description ?? string.Empty,
            CompetencyLevelIds = currentTaskOfPeriod.CompetencyLevelTaskMappings.Select(a => a.CompetencyLevelId).ToList(),
            RoleAndInfluence = currentTaskOfPeriod.RoleAndInfluence,
            SuccessRate = currentTaskOfPeriod.SuccessRate,
            TaskUserMentionIds = currentTaskOfPeriod.TaskUserMentions.Where(a => !a.Manager).Select(a => a.UserId).ToList(),
            Title = currentTaskOfPeriod.Title,
            PerformanceManagementPeriodUserMappingId = currentTaskOfPeriod.PerformanceManagementPeriodUserMappingId
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

    [BindProperty]
    public TaskOfPeriodRequestDto TaskOfPeriod { get; set; } = default!;

    [BindProperty(SupportsGet = true)]
    public Guid CompetencyId { get; set; }
    public List<string> ManagerNames { get; set; } = null!;
    public List<CompetencyLevelSelectListResponseDto> CompetencyLevels { get; set; } = new();

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        var currentTaskOfPeriod = await _context.TaskOfPeriods
            .Include(a => a.CompetencyLevelTaskMappings)
            .ThenInclude(a => a.CompetencyLevel.Competency)
            .Include(a => a.TaskUserMentions)
            .FirstOrDefaultAsync(a => a.Id == TaskOfPeriod.Id);

        if (currentTaskOfPeriod == null)
        {
            return NotFound();
        }

        _context.TaskOfPeriods.Remove(currentTaskOfPeriod);
        var id = Guid.NewGuid();
        var managers = await _context.UserManagerMappings.Where(a => a.UserId == HttpContext.User.UserId())
            .ToListAsync();
        var taskOfPeriod = new TaskOfPeriod
        {
            Id = id,
            Title = TaskOfPeriod.Title,
            Active = true,
            Description = TaskOfPeriod.Description,
            UserId = HttpContext.User.UserId(),
            DutyInTask = DutyEnum.SelfTaskWriter,
            RoleAndInfluence = TaskOfPeriod.RoleAndInfluence,
            SuccessRate = TaskOfPeriod.SuccessRate,
            Type = TaskType.SelfAssessment,
            PerformanceManagementPeriodUserMappingId = TaskOfPeriod.PerformanceManagementPeriodUserMappingId,
            CompetencyLevelTaskMappings = TaskOfPeriod.CompetencyLevelIds.Select(competencyLevelId => new CompetencyLevelTaskMapping
            {
                Active = true,
                CompetencyLevelId = competencyLevelId,
                TaskOfPeriodId = id,
                Title = string.Empty
            }).ToList(),
            TaskUserMentions = TaskOfPeriod.TaskUserMentionIds.Select(taskUserMentionId => new TaskUserMention
            {
                Title = string.Empty,
                Active = true,
                TaskOfPeriodId = id,
                UserId = taskUserMentionId,
                Manager = false
            }).ToList()
        };
        foreach (var userManagerMapping in managers)
        {
            taskOfPeriod.TaskUserMentions.Add(new TaskUserMention
            {
                Title = string.Empty,
                Active = true,
                TaskOfPeriodId = id,
                UserId = userManagerMapping.ManagerId,
                Manager = true
            });
        }
        await _context.TaskOfPeriods.AddAsync(taskOfPeriod);

        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }

    public async Task<JsonResult> OnGetCompetencyLevels()
    {
        var competencyLevels = await _context.CompetencyLevels
            .Where(a => a.CompetencyId == CompetencyId).OrderBy(a => a.Level).Select(a => new
            {
                a.Id,
                a.Title,
                a.Description
            }).ToListAsync();
        return new JsonResult(competencyLevels);
    }
}