using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PerformanceManagementSystem.Data;
using PerformanceManagementSystem.Data.Views.ManagerEvaluations;
using PerformanceManagementSystem.Data.Views.TaskOfPeriods;

namespace PerformanceManagementSystem.Areas.PerformanceManagement.Pages.ManagerAssessments;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;
    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<ManagerTaskOfPeriodListResponseDto> TaskOfPeriods { get; set; } = default!;
    public ManagerEvaluationResponseDto? ManagerEvaluation { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = HttpContext.User.UserId();
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

        var taskOfPeriodUserMentions = await _context.TaskUserMentions
                .Include(a => a.TaskOfPeriod)
                .ThenInclude(a => a.PerformanceManagementPeriodUserMapping.User)
                .Where(a => a.UserId == userId && a.Manager).Select(a => new ManagerTaskOfPeriodForMappingResponseDto
                {
                    UserId = a.TaskOfPeriod.PerformanceManagementPeriodUserMapping.UserId,
                    MainTaskId = a.TaskOfPeriod.Id,
                    Title = a.TaskOfPeriod.Title,
                    Fullname = $"{a.TaskOfPeriod.PerformanceManagementPeriodUserMapping.User.Firstname} " +
                               $"{a.TaskOfPeriod.PerformanceManagementPeriodUserMapping.User.Lastname} - " +
                               $"{a.TaskOfPeriod.PerformanceManagementPeriodUserMapping.User.UserName}"
                }).ToListAsync();



        foreach (var item in taskOfPeriodUserMentions)
        {
            var submitted = await _context.TaskOfPeriods.FirstOrDefaultAsync(a =>
                a.MainTaskOfPeriodId == item.MainTaskId && a.UserId == userId);
            if (submitted != null)
            {
                item.Id = submitted.Id;
                item.SuccessRate = submitted.SuccessRate;
                item.DutyInTask = submitted.DutyInTask;
            }
        }
        var grouped = taskOfPeriodUserMentions.GroupBy(a => new { a.UserId, a.Fullname })
            .Select(a => new { Id = a.Key, Values = a.ToList() }).Select(a => new ManagerTaskOfPeriodListResponseDto
            {
                Fullname = a.Id.Fullname,
                UserId = a.Id.UserId,
                Items = a.Values.Select(b => new ManagerTaskOfPeriodItemResponseDto
                {
                    MainTaskId = b.MainTaskId,
                    Title = b.Title,
                    DutyInTask = b.DutyInTask,
                    Id = b.Id,
                    SuccessRate = b.SuccessRate
                }).ToList()
            });
        TaskOfPeriods = grouped.ToList();
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