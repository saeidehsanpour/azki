using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PerformanceManagementSystem.Data.Views.ManagerEvaluations;
using PerformanceManagementSystem.Data.Views.Users;

namespace PerformanceManagementSystem.Areas.PerformanceManagement.Pages.ManagerReports;

public class IndexModel : PageModel
{
    private readonly Data.ApplicationDbContext _context;
    public IndexModel(Data.ApplicationDbContext context)
    {
        _context = context;
    }
    public IList<ManagerResponseDto> Managers { get; set; } = default!;
    public ManagerEvaluationResponseDto? ManagerEvaluation { get; set; }
    public async Task<IActionResult> OnGetAsync()
    {
        var userId = HttpContext.User.UserId();

        Managers = await _context.UserManagerMappings
            .Include(a => a.Manager.Position)
            .Where(a => a.UserId == userId).Select(a => new ManagerResponseDto
            {
                Fullname = $"{a.Manager.Firstname} {a.Manager.Lastname}",
                Id = a.ManagerId,
                Position = a.Manager.Position.Title
            }).ToListAsync();

        var isManager = await _context.UserManagerMappings
            .Where(a => a.ManagerId == userId)
            .Select(a => a.UserId).ToListAsync();

        if (isManager.Any())
        {
            var managerEvaluations = await _context.ManagerEvaluations
                .Include(a => a.PerformanceManagementPeriodUserMapping)
                .Include(a => a.ManagerEvaluationAnswers)
                .ThenInclude(a => a.ManagerEvaluationQuestion)
                .Where(a => a.ManagerId == userId && isManager.Contains(a.PerformanceManagementPeriodUserMapping.UserId)).ToListAsync();

            var averages = managerEvaluations.SelectMany(a => a.ManagerEvaluationAnswers).GroupBy(a => a.ManagerEvaluationQuestionId)
                .Select(a => new { Id = a.Key, Values = a.ToList() });


            ManagerEvaluation = new ManagerEvaluationResponseDto
            {
                ManagerEvaluationStringQuestions = managerEvaluations.Select(a =>
                    new ManagerEvaluationStringQuestionResponseDto
                    {
                        Continue = a.Continue,
                        ShouldImprove = a.ShouldImprove
                    }).ToList()
            };

            foreach (var average in averages)
            {
                if (!average.Values.Any())
                    continue;

                ManagerEvaluation.ManagerEvaluationAverageOfAnswers.Add(new ManagerEvaluationAverageOfAnswerResponseDto
                {
                    Average = average.Values.Average(a => a.Answer),
                    QuestionName = average.Values.First().ManagerEvaluationQuestion.Question,
                    Answers = average.Values.Select(a => a.Answer).ToList()
                });
            }

        }


        return Page();
    }
}