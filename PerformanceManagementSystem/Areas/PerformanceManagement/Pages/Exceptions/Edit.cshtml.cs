using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PerformanceManagementSystem.Data;
using PerformanceManagementSystem.Data.Enums;
using PerformanceManagementSystem.Data.Models;
using PerformanceManagementSystem.Data.Views.Competencies;
using PerformanceManagementSystem.Data.Views.UserExceptions;

namespace PerformanceManagementSystem.Areas.PerformanceManagement.Pages.Exceptions;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public EditModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public UserExceptionRequestDto UserException { get; set; } = default!;
    public IList<CompetencyResponseDto> Competencies { get; set; } = default!;
    public async Task<IActionResult> OnGetAsync(Guid? userId)
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
            }).FirstOrDefaultAsync();

        if (currentUser == null)
        {
            return NotFound();
        }

        var competencies = await _context.CompetencyCategoryPositionMappings
            .Include(a => a.CompetencyCategory.Competencies)
            .Where(a => a.PositionId == currentUser.PositionId && a.Active && a.CompetencyCategory.Competencies.Any(b => b.Active))
            .SelectMany(a => a.CompetencyCategory.Competencies).ToListAsync();

        Competencies = competencies.Select(a => new CompetencyResponseDto
        {
            Title = a.Title,
            Active = a.Active,
            Id = a.Id,
            CompetencyCategoryId = a.CompetencyCategoryId,
            Description = a.Description,
            UpdatedDate = a.UpdatedDate
        }).ToList();

        var currentException = await _context.UserExceptions
            .Include(a => a.UserExceptionCompetencyMappings)
            .ThenInclude(a => a.Competency)
            .FirstOrDefaultAsync(a =>
            a.ManagerId == HttpContext.User.UserId() && a.PerformanceManagementPeriodUserMappingId == performanceManagementPeriodUserMapping.Id);
        var items = competencies.Select(a => new UserExceptionItemRequestDto
        {
            Description = string.Empty,
            CompetencyName = a.Title,
            CompetencyId = a.Id,
            Use = true
        }).ToList();

        if (currentException != null)
        {
            foreach (var item in items)
            {
                var exist = currentException.UserExceptionCompetencyMappings.FirstOrDefault(a =>
                    a.CompetencyId == item.CompetencyId);

                if (exist == null)
                {
                    item.Use = false;
                }
                else
                {
                    item.Description = exist.Description;
                    item.Use = true;
                }
            }
            UserException = new UserExceptionRequestDto
            {
                Id = currentException.Id,
                PerformanceManagementPeriodUserMappingId = performanceManagementPeriodUserMapping.Id,
                Continue = currentException.Continue,
                Items = items
            };
        }
        else
        {
            var continueString = string.Empty;
            var currentSelfAssessmentOverall = await _context.TaskOfPeriods
                .FirstOrDefaultAsync(a => a.PerformanceManagementPeriodUserMappingId
                                          == performanceManagementPeriodUserMapping.Id
                                          && a.UserId == HttpContext.User.UserId() && a.Type == TaskType.ManagerAssessmentOverall);
            if (currentSelfAssessmentOverall != null)
            {
                continueString = currentSelfAssessmentOverall.Continue;
            }
            UserException = new UserExceptionRequestDto
            {
                PerformanceManagementPeriodUserMappingId = performanceManagementPeriodUserMapping.Id,
                Continue = continueString,
                Items = items
            };
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        var currentException = await _context.UserExceptions
            .Include(a => a.UserExceptionCompetencyMappings)
            .FirstOrDefaultAsync(a =>
                a.ManagerId == HttpContext.User.UserId() && a.PerformanceManagementPeriodUserMappingId == UserException.PerformanceManagementPeriodUserMappingId);

        var id = Guid.NewGuid();
        if (currentException != null)
        {
            _context.UserExceptions.Remove(currentException);
        }
        var userException = new UserException
        {
            Id = id,
            Title = string.Empty,
            Active = true,
            Continue = UserException.Continue,
            ManagerId = HttpContext.User.UserId(),
            PerformanceManagementPeriodUserMappingId = UserException.PerformanceManagementPeriodUserMappingId,
            UserExceptionCompetencyMappings = UserException.Items.Where(a => a.Use).Select(a => new UserExceptionCompetencyMapping
            {
                UserExceptionId = id,
                CompetencyId = a.CompetencyId,
                Description = a.Description
            }).ToList()
        };

        await _context.UserExceptions.AddAsync(userException);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}