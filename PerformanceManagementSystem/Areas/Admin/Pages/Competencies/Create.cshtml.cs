using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PerformanceManagementSystem.Data;
using PerformanceManagementSystem.Data.Models;
using PerformanceManagementSystem.Data.Views.Competencies;

namespace PerformanceManagementSystem.Areas.Admin.Pages.Competencies;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    public CreateModel(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IActionResult> OnGetAsync(Guid competencyCategoryId)
    {
        if (competencyCategoryId == Guid.Empty)
            return NotFound();

        var isExist = await _context.CompetencyCategories.AnyAsync(a => a.Id == competencyCategoryId);
        if (!isExist)
            return NotFound();

        Competency = new CompetencyRequestDto
        {
            CompetencyCategoryId = competencyCategoryId,
            Active = true,
            Title = "",
            Id = Guid.NewGuid()
        };

        return Page();
    }

    [BindProperty]
    public CompetencyRequestDto Competency { get; set; } = default!;


    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var competency = _mapper.Map<CompetencyRequestDto, Competency>(Competency);
        _context.Competencies.Add(competency);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index", new { competencyCategoryId = Competency.CompetencyCategoryId });
    }
}