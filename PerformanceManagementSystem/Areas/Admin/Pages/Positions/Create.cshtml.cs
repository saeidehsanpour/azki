using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PerformanceManagementSystem.Data;
using PerformanceManagementSystem.Data.Models;
using PerformanceManagementSystem.Data.Views.Positions;

namespace PerformanceManagementSystem.Areas.Admin.Pages.Positions;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    public CreateModel(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IActionResult> OnGetAsync(Guid organizationId)
    {
        if (organizationId == Guid.Empty)
            return NotFound();

        var isExist = await _context.Organizations.AnyAsync(a => a.Id == organizationId);
        if (!isExist)
            return NotFound();

        Position = new PositionRequestDto
        {
            OrganizationId = organizationId,
            Active = true,
            Title = "",
            Id = Guid.NewGuid()
        };

        return Page();
    }

    [BindProperty]
    public PositionRequestDto Position { get; set; } = default!;


    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var position = _mapper.Map<PositionRequestDto, Position>(Position);
        _context.Positions.Add(position);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index", new { organizationId = Position.OrganizationId });
    }
}