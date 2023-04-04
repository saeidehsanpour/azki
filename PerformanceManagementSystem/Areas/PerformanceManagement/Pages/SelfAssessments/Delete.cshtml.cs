using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PerformanceManagementSystem.Data;
using PerformanceManagementSystem.Data.Views;
using PerformanceManagementSystem.Data.Views.CompetencyLevels;
using PerformanceManagementSystem.Data.Views.TaskOfPeriods;

namespace PerformanceManagementSystem.Areas.PerformanceManagement.Pages.SelfAssessments;

public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _context;
    public DeleteModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        var timeId = HttpContext.Request.Cookies["PerformanceManagementCookie"];
        if (timeId == null || id == null)
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

        var currentTaskOfPeriod = await _context.TaskOfPeriods
            .Include(a => a.CompetencyLevelTaskMappings)
            .ThenInclude(a => a.CompetencyLevel.Competency)
            .Include(a => a.TaskUserMentions)
            .ThenInclude(a => a.User)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (currentTaskOfPeriod == null)
        {
            return NotFound();
        }

        ViewData["TaskUserMentionIds"] = new SelectList(currentTaskOfPeriod.TaskUserMentions.Where(a => !a.Manager).Select(a => new UserResponseDto
        {
            Id = a.UserId,
            Name = $"{a.User.Firstname} {a.User.Lastname} ({a.User.Email})"
        }), "Id", "Name", currentTaskOfPeriod.TaskUserMentions.Where(a => !a.Manager).Select(a => a.UserId).ToList());

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

    public TaskOfPeriodRequestDto TaskOfPeriod { get; set; } = default!;
    public List<string> ManagerNames { get; set; } = null!;
    public List<CompetencyLevelSelectListResponseDto> CompetencyLevels { get; set; } = new();

    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        if (id == null)
        {
            return RedirectToPage("./Index");
        }

        var currentTaskOfPeriod = await _context.TaskOfPeriods
            .Include(a => a.CompetencyLevelTaskMappings)
            .ThenInclude(a => a.CompetencyLevel.Competency)
            .Include(a => a.TaskUserMentions)
            .ThenInclude(a => a.User)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (currentTaskOfPeriod == null)
        {
            return NotFound();
        }

        _context.TaskOfPeriods.Remove(currentTaskOfPeriod);

        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}