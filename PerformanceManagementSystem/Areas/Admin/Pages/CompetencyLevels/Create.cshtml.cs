using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PerformanceManagementSystem.Data;
using PerformanceManagementSystem.Data.Models;
using PerformanceManagementSystem.Data.Views.CompetencyLevels;

namespace PerformanceManagementSystem.Areas.Admin.Pages.CompetencyLevels;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    public CreateModel(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IActionResult> OnGetAsync(Guid competencyId)
    {
        if (competencyId == Guid.Empty)
            return NotFound();

        var isExist = await _context.Competencies.AnyAsync(a => a.Id == competencyId);
        if (!isExist)
            return NotFound();

        CompetencyLevel = new CompetencyLevelRequestDto
        {
            CompetencyId = competencyId,
            Active = true,
            Title = "",
            Id = Guid.NewGuid(),
            Level = 0
        };

        return Page();
    }

    [BindProperty]
    public CompetencyLevelRequestDto CompetencyLevel { get; set; } = default!;


    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var competency = _mapper.Map<CompetencyLevelRequestDto, CompetencyLevel>(CompetencyLevel);
        _context.CompetencyLevels.Add(competency);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index", new { competencyId = CompetencyLevel.CompetencyId });
    }
}