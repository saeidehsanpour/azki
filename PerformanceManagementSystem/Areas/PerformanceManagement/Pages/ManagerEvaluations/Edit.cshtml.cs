using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PerformanceManagementSystem.Data;
using PerformanceManagementSystem.Data.Models;
using PerformanceManagementSystem.Data.Views.ManagerEvaluations;

namespace PerformanceManagementSystem.Areas.PerformanceManagement.Pages.ManagerEvaluations;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;
    public EditModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> OnGetAsync(Guid managerId)
    {
        var timeId = HttpContext.Request.Cookies["PerformanceManagementCookie"];
        if (timeId == null)
        {
            return RedirectToPage("./Index");
        }

        var currentUser = await _context.UserManagerMappings
            .Include(a => a.Manager)
            .Include(a => a.User)
            .Where(a => a.UserId == HttpContext.User.UserId() && a.ManagerId == managerId)
            .Select(a => new
            {
                a.User.Id,
                ManagerName = $"{a.Manager.Firstname} {a.Manager.Lastname}",
            }).FirstOrDefaultAsync();
        if (currentUser == null)
        {
            return NotFound();
        }

        var performanceManagementPeriodUserMapping = await _context.PerformanceManagementPeriodUserMappings
            .Where(a => a.PerformanceManagementPeriodId == Guid.Parse((ReadOnlySpan<char>)timeId)
                        && a.UserId == currentUser.Id).FirstOrDefaultAsync();

        if (performanceManagementPeriodUserMapping == null)
        {
            return NotFound();
        }

        var existForThisManager = await _context.ManagerEvaluations
            .Include(a => a.ManagerEvaluationAnswers)
            .ThenInclude(a => a.ManagerEvaluationQuestion)
            .FirstOrDefaultAsync(a =>
            a.ManagerId == managerId &&
            a.PerformanceManagementPeriodUserMappingId == performanceManagementPeriodUserMapping.Id);

        if (existForThisManager != null)
        {
            ManagerEvaluation = new ManagerEvaluationRequestDto
            {
                Id = existForThisManager.Id,
                PerformanceManagementPeriodUserMappingId = performanceManagementPeriodUserMapping.Id,
                Continue = existForThisManager.Continue,
                ManagerId = managerId,
                ShouldImprove = existForThisManager.ShouldImprove,
                ManagerEvaluationAnswerRequests = existForThisManager.ManagerEvaluationAnswers.OrderBy(a => a.ManagerEvaluationQuestion.DisplayOrder).Select(a => new ManagerEvaluationAnswerRequestDto
                {
                    ManagerEvaluationQuestionId = a.ManagerEvaluationQuestionId,
                    Answer = a.Answer,
                    QuestionName = a.ManagerEvaluationQuestion.Question
                }).ToList()
            };
        }
        else
        {
            var questions = await _context.ManagerEvaluationQuestions
                .Where(a => a.Active && !a.Deleted)
                .OrderBy(a => a.DisplayOrder).ToListAsync();
            ManagerEvaluation = new ManagerEvaluationRequestDto
            {
                PerformanceManagementPeriodUserMappingId = performanceManagementPeriodUserMapping.Id,
                Continue = string.Empty,
                ManagerId = managerId,
                ShouldImprove = string.Empty,
                ManagerEvaluationAnswerRequests = questions.Select(a => new ManagerEvaluationAnswerRequestDto
                {
                    ManagerEvaluationQuestionId = a.Id,
                    Answer = 1,
                    QuestionName = a.Question
                }).ToList()
            };
        }

        ManagerName = currentUser.ManagerName;

        return Page();
    }

    [BindProperty]
    public ManagerEvaluationRequestDto ManagerEvaluation { get; set; } = default!;
    public string ManagerName { get; set; } = null!;

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (ManagerEvaluation.Id != Guid.Empty)
        {
            var managerEvaluation = await _context.ManagerEvaluations.FirstOrDefaultAsync(a => a.Id == ManagerEvaluation.Id);
            if (managerEvaluation != null)
            {
                _context.ManagerEvaluations.Remove(managerEvaluation);
            }
        }
        var id = Guid.NewGuid();

        var newManagerEvaluation = new ManagerEvaluation
        {
            Id = id,
            Title = string.Empty,
            Active = true,
            Continue = ManagerEvaluation.Continue,
            ManagerId = ManagerEvaluation.ManagerId,
            PerformanceManagementPeriodUserMappingId = ManagerEvaluation.PerformanceManagementPeriodUserMappingId,
            ShouldImprove = ManagerEvaluation.ShouldImprove,
            ManagerEvaluationAnswers = ManagerEvaluation.ManagerEvaluationAnswerRequests.Select(a =>
                new ManagerEvaluationAnswer
                {
                    ManagerEvaluationId = id,
                    Answer = a.Answer,
                    ManagerEvaluationQuestionId = a.ManagerEvaluationQuestionId
                }).ToList()
        };

        await _context.ManagerEvaluations.AddAsync(newManagerEvaluation);

        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}