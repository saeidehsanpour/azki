using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PerformanceManagementSystem.Data;
using PerformanceManagementSystem.Data.Enums;
using PerformanceManagementSystem.Data.Models;
using PerformanceManagementSystem.Data.Views;
using PerformanceManagementSystem.Data.Views.TaskOfPeriods;

namespace PerformanceManagementSystem.Areas.PerformanceManagement.Pages.SelfAssessments;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;
    public CreateModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> OnGetAsync()
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
            .Include(a => a.TaskOfPeriods)
            .Where(a => a.PerformanceManagementPeriodId == Guid.Parse((ReadOnlySpan<char>)timeId)
                        && a.UserId == currentUser.Id).FirstOrDefaultAsync();

        if (performanceManagementPeriodUserMapping == null 
            || performanceManagementPeriodUserMapping.TaskOfPeriods.Where(a => a.Active && !a.Deleted).ToList().Count == 5
            || performanceManagementPeriodUserMapping.PerformanceManagementPeriod.SelfScoreEndDate.Date < DateTime.UtcNow.Date)
        {
            return RedirectToPage("./Index");
        }

        ViewData["TaskUserMentionIds"] = new SelectList(_context.Users.Where(a => a.PositionId == currentUser.PositionId && a.Id != currentUser.Id).Select(a => new UserResponseDto
        {
            Id = a.Id,
            Name = $"{a.Firstname} {a.Lastname} ({a.Email})"
        }), "Id", "Name");

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
            Description = string.Empty,
            CompetencyLevelIds = new List<Guid>(),
            RoleAndInfluence = string.Empty,
            SuccessRate = SuccessRateEnum.Normal,
            TaskUserMentionIds = new List<Guid>(),
            Title = string.Empty,
            PerformanceManagementPeriodUserMappingId = performanceManagementPeriodUserMapping.Id
        };
        return Page();
    }

    [BindProperty]
    public TaskOfPeriodRequestDto TaskOfPeriod { get; set; } = default!;

    [BindProperty(SupportsGet = true)]
    public Guid CompetencyId { get; set; }
    public List<string> ManagerNames { get; set; } = null!;

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

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
            .Where(a => a.CompetencyId == CompetencyId).Select(a => new
            {
                a.Id,
                a.Title,
                a.Description
            }).ToListAsync();
        return new JsonResult(competencyLevels);
    }
}