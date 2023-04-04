using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PerformanceManagementSystem.Data;
using PerformanceManagementSystem.Data.Models;
using PerformanceManagementSystem.Data.Views.CompetencyCategories;

namespace PerformanceManagementSystem.Areas.Admin.Pages.CompetencyCategories;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    public CreateModel(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    [BindProperty]
    public CompetencyCategoryRequestDto CompetencyCategory { get; set; } = default!;


    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var competencyCategory = _mapper.Map<CompetencyCategoryRequestDto, CompetencyCategory>(CompetencyCategory);
        _context.CompetencyCategories.Add(competencyCategory);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}