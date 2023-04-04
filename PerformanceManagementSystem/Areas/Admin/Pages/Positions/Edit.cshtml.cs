using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PerformanceManagementSystem.Data;
using PerformanceManagementSystem.Data.Models;
using PerformanceManagementSystem.Data.Views.Positions;

namespace PerformanceManagementSystem.Areas.Admin.Pages.Positions;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    public EditModel(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [BindProperty]
    public PositionRequestDto Position { get; set; } = default!;
    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var position = await _context.Positions.Select(a => new PositionRequestDto
        {
            Id = a.Id,
            Active = a.Active,
            Title = a.Title,
            OrganizationId = a.OrganizationId,
        }).FirstOrDefaultAsync(m => m.Id == id);
        if (position == null)
        {
            return NotFound();
        }
        Position = position;
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var position = _mapper.Map<PositionRequestDto, Position>(Position);
        position.UpdatedDate = DateTimeOffset.UtcNow;
        _context.Positions.Update(position);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PositionExists(Position.Id))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToPage("./Index", new { organizationId = position.OrganizationId });
    }

    private bool PositionExists(Guid id)
    {
        return (_context.Positions?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}